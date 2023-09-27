using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public partial class BookCaseRepository : ObservableObject, BaseRepositoryInterface
    {
        private ObservableCollection<BookCase> _bookCases { get; } = new();

        public BookCaseRepository() 
        {
            

        }

        public void AddBookCase(BookCaseType bookCaseType)
        {
            BookCase bookCase = new BookCase();
            bookCase.BookCaseType = bookCaseType;
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO BOOKCASE(BookCaseType)"
                        + "VALUES(@type) SELECT SCOPE_IDENTITY()", con);
                    cmd.Parameters.Add("@type", SqlDbType.Int).Value = (int)bookCase.BookCaseType;
                    bookCase.ID = Convert.ToInt32(cmd.ExecuteScalar());
                    _bookCases.Add(bookCase);
                    con.Close();

                }

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }

        public void DeleteBookCase(BookCase bookCase)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE BOOKCASE WHERE ID = @id", con);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = bookCase.ID;
                    con.Close();
                }
            }
            catch (Exception)
            {

                throw new Exception("Bookcase not found");
            }
        }

        public void GetAllBookCases()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ID, BookCaseType FROM BOOKCASE", con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            BookCase bookCase = new();
                            bookCase.ID = Convert.ToInt32(dr[nameof(bookCase.ID)]);
                            bookCase.BookCaseType =(BookCaseType)Convert.ToInt32(dr[nameof(BookCaseType)]);
                            _bookCases.Add(bookCase);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public BookCase GetBookcase(int id) 
        {
            BookCase result = _bookCases.FirstOrDefault(x => x.ID == id);


                return result != null ? result : throw new Exception("Kunne ikke finde den reol du ledte efter");
        }


        public List<BookCase> FindAvailableBookCases(DateTime startDate, DateTime finalDate, List<BookCase> bookCases)
        {
            TimeSpan ts = new TimeSpan(1, 0, 0, 0);
            List<BookCase> BookCasesFromDB = new();

            try
            {
                // hvis start datoen for udlejningen er før dagen før så er den ikke gyldig og kaster derfor
                // en argument exception
                if (startDate < DateTime.Now - ts)
                {
                    throw new ArgumentException("Start dato kan ikke være før idag.");
                } // ellers hvis udlejningens slut dato er før startdatoen så kaster den også en exception
                else if (finalDate < startDate)
                {
                    throw new ArgumentException("Slut dato kan ikke være før start dato");
                }
                // hvis der ikke er nogen reoler i udlejningen, så kaster den en exception
                if (bookCases.IsNullOrEmpty())
                {
                    throw new ArgumentException("en udlejning skal indeholde reoler, aflåste skabe eller hylder i aflåste skabe");
                }


                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    // sql query der får alle reoler ud som er ledige i den gældende udlejningens periode
                    SqlCommand cmd = new SqlCommand(@"SELECT BC.ID, BC.BookCaseType
                            FROM BOOKCASE BC LEFT JOIN
                            BOOKCASE_RENTAL BCR ON BC.ID = BCR.BookCaseID 
                            LEFT JOIN RENTAL R ON BCR.RENTALID = R.ID 
                            WHERE BC.ID NOT IN (
                            SELECT BC.ID
                            FROM BOOKCASE BC
                            INNER JOIN BOOKCASE_RENTAL BCR ON BC.ID = BCR.BookCaseID
                            INNER JOIN RENTAL R ON BCR.RENTALID = R.ID
                            WHERE (R.StartDate BETWEEN @startDate AND @finalDate)
                            OR (R.FinalDate BETWEEN @startDate AND @finalDate))", con);
                    cmd.Parameters.Add("@startDate", SqlDbType.DateTime2).Value = startDate;
                    cmd.Parameters.Add("@finalDate", SqlDbType.DateTime2).Value = finalDate;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            BookCase bc = new BookCase()
                            {
                                ID = Convert.ToInt32(dr[nameof(BookCase.ID)]),
                                BookCaseType = (BookCaseType)Convert.ToInt32(dr[nameof(BookCaseType)])
                            };

                            BookCasesFromDB.Add(bc);
                        }
                    }

                    con.Close();
                    // hvis der ikke er nogle reoler i bookcasesfromdb så er der ikke nogen ledige
                    // reoler i den gældende udlejnings periode
                    if (BookCasesFromDB.IsNullOrEmpty())
                    {
                        throw new ArgumentException("der er intet ledigt på for den valgte lejeperiode");
                    }

                }

                foreach (BookCase bookCase in bookCases)
                {
                    try
                    {
                        bookCase.ID = BookCasesFromDB.FirstOrDefault(x => x.BookCaseType == bookCase.BookCaseType).ID;
                        if (bookCase.ID == default)
                        {
                            throw new ArgumentException("Der er ikke nok ledige reoler af typen: "
                                + $" {bookCase.BookCaseType} ledige i den valgte periode");
                        }
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.Message);
                    }
                   
                }
            } // hvis der er kastet en exception så bliver den fanget her og bliver kastet som en ny exception 
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            //  giver reolerne en ID så de matcher i databasen og  samtidigt checker om der er nok reoler ledige
            // til den givne udlejning


            return bookCases;
        }

        public void ReserveBookCases(List<BookCase> bookCases, int rentalID)
        {
            foreach (BookCase bookCase in bookCases)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                    {
                        con.Open();

                        // tilføjer relationen mellem reol og den gældende udlejning sådan at
                        // andre ikke kan leje dem i samme periode
                        SqlCommand cmd = new SqlCommand("INSERT INTO BOOKCASE_RENTAL(BookCaseID, RENTALID)"
                            + "VALUES(@bookCaseID,@rentalID)", con);
                        cmd.Parameters.Add("@bookCaseID", SqlDbType.Int).Value = bookCase.ID;
                        cmd.Parameters.Add("@rentalID", SqlDbType.Int).Value = rentalID;
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }
            }
        }
    }
}
