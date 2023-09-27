using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Documents;

namespace Reolmarkedet.Models
{
    public partial class RentalRepository : ObservableObject, BaseRepositoryInterface
    {
        private ObservableCollection<Rental> _rentals = new();
        public RentalRepository() { }
        private Rental _tempRental = new();


        public void AddRental(DateTime startDate, DateTime finalDate, int tenantID,
            List<BookCase> bookCases)
        {
            TimeSpan ts = new TimeSpan(1, 0, 0, 0);
            Rental rental = new Rental();
            rental.StartDate = startDate;
            rental.FinalDate = finalDate;
            rental.Settled = default;
            rental.ThingsSoldCounter = default;
            rental.TotalAmountSoldFor = default;
            rental.TenantID = tenantID;
            //List<BookCase> BookCasesFromDB = new();
            BookCaseRepository bookCaseRepository = new BookCaseRepository();
            List<BookCase> bookCasesForReservation = new List<BookCase>();
            
            //try
            //{
            //    // hvis start datoen for udlejningen er før dagen før så er den ikke gyldig og kaster derfor
            //    // en argument exception
            //    if (rental.StartDate < DateTime.Now - ts) 
            //    {
            //        throw new ArgumentException("Start dato kan ikke være før idag.");
            //    } // ellers hvis udlejningens slut dato er før startdatoen så kaster den også en exception
            //    else if (rental.FinalDate < rental.StartDate)
            //    {
            //        throw new ArgumentException("Slut dato kan ikke være før start dato");
            //    }
            //    // hvis der ikke er nogen reoler i udlejningen, så kaster den en exception
            //    if (bookCases.IsNullOrEmpty())
            //    {
            //        throw new ArgumentException("en udlejning skal indeholde reoler, aflåste skabe eller hylder i aflåste skabe");
            //    }


            //    using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
            //    {
            //        con.Open();
            //        // sql query der får alle reoler ud som er ledige i den gældende udlejningens periode
            //        SqlCommand cmd = new SqlCommand(@"SELECT BC.ID, BC.BookCaseType
            //                FROM BOOKCASE BC LEFT JOIN
            //                BOOKCASE_RENTAL BCR ON BC.ID = BCR.BookCaseID 
            //                LEFT JOIN RENTAL R ON BCR.RENTALID = R.ID 
            //                WHERE BC.ID NOT IN (
            //                SELECT BC.ID
            //                FROM BOOKCASE BC
            //                INNER JOIN BOOKCASE_RENTAL BCR ON BC.ID = BCR.BookCaseID
            //                INNER JOIN RENTAL R ON BCR.RENTALID = R.ID
            //                WHERE (R.StartDate BETWEEN @startDate AND @finalDate)
            //                OR (R.FinalDate BETWEEN @startDate AND @finalDate))", con);
            //        cmd.Parameters.Add("@startDate", SqlDbType.DateTime2).Value = rental.StartDate;
            //        cmd.Parameters.Add("@finalDate", SqlDbType.DateTime2).Value = rental.FinalDate;
            //        using (SqlDataReader dr = cmd.ExecuteReader())
            //        {
            //            while (dr.Read())
            //            {
            //                BookCase bc = new BookCase()
            //                {
            //                    ID = Convert.ToInt32(dr[nameof(BookCase.ID)]),
            //                    BookCaseType = (BookCaseType)Convert.ToInt32(dr[nameof(BookCaseType)])
            //                };

            //                BookCasesFromDB.Add(bc);
            //            }
            //        }

            //        con.Close();
            //        // hvis der ikke er nogle reoler i bookcasesfromdb så er der ikke nogen ledige
            //        // reoler i den gældende udlejnings periode
            //        if (BookCasesFromDB.IsNullOrEmpty())
            //        {
            //            throw new ArgumentException("der er intet ledigt på for den valgte lejeperiode");
            //        }

            //    }
            //} // hvis der er kastet en exception så bliver den fanget her og bliver kastet som en ny exception 
            //catch (Exception e)
            //{
            //    throw new Exception(e.Message);
            //}
            ////  giver reolerne en ID så de matcher i databasen og  samtidigt checker om der er nok reoler ledige
            //// til den givne udlejning
            //foreach (BookCase bookCase in bookCases)
            //{
            //    bookCase.ID = BookCasesFromDB.FirstOrDefault(x => x.BookCaseType == bookCase.BookCaseType).ID;
            //    if (bookCase.ID == default)
            //    {
            //        throw new ArgumentException("Der er ikke nok ledige reoler af typen"
            //            +$" {bookCase.BookCaseType} ledige i den valgte periode");
            //    }
            //}


            try
            {
                bookCasesForReservation = bookCaseRepository.FindAvailableBookCases(rental.StartDate,rental.FinalDate, bookCases);
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    // tilføjer udlejningen til databasen og _rentals listen
                    // dette bliver kun gjort hvis det forgående ikke har kastet en exception
                    SqlCommand cmd = new SqlCommand("INSERT INTO RENTAL(StartDate, "
                        + "FinalDate, ThingsSoldCounter, TotalAmountSoldFor, Settled, TenantID)"
                        + "VALUES(@startDate,@finalDate,@thingsSoldCounter"
                        + ",@totalAmountSoldFor,@settled,@tenantID) SELECT SCOPE_IDENTITY()", con);
                    cmd.Parameters.Add("@startDate", SqlDbType.DateTime2).Value = rental.StartDate;
                    cmd.Parameters.Add("@finalDate", SqlDbType.DateTime2).Value = rental.FinalDate;
                    cmd.Parameters.Add("@thingsSoldCounter", SqlDbType.Int).Value = rental.ThingsSoldCounter;
                    cmd.Parameters.Add("@totalAmountSoldFor", SqlDbType.Float).Value = rental.TotalAmountSoldFor;
                    cmd.Parameters.Add("@settled", SqlDbType.Bit).Value = rental.Settled;
                    cmd.Parameters.Add("@tenantID", SqlDbType.Int).Value = rental.TenantID;
                    rental.ID = Convert.ToInt32(cmd.ExecuteScalar());
                    _rentals.Add(rental);
                    con.Close();
                    bookCaseRepository.ReserveBookCases(bookCasesForReservation, rental.ID);
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

            //foreach (BookCase bookCase in bookCases)
            //{
            //    try
            //    {
            //        using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
            //        {
            //            con.Open();

            //            // tilføjer relationen mellem reol og den gældende udlejning sådan at
            //            // andre ikke kan leje dem i samme periode
            //            SqlCommand cmd = new SqlCommand("INSERT INTO BOOKCASE_RENTAL(BookCaseID, RENTALID)"
            //                + "VALUES(@bookCaseID,@rentalID)", con);
            //            cmd.Parameters.Add("@bookCaseID", SqlDbType.Int).Value = bookCase.ID;
            //            cmd.Parameters.Add("@rentalID", SqlDbType.Int).Value = rental.ID;
            //            cmd.ExecuteNonQuery();
            //            con.Close();
            //        }
            //    }
            //    catch (Exception e)
            //    {

            //        throw new Exception(e.Message);
            //    }
            //}

        }


        public void GetAllRentals()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM RENTAL", con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Rental tempRental = new()
                            {
                                ID = Convert.ToInt32(dr[nameof(Rental.ID)]),
                                StartDate = Convert.ToDateTime(dr[nameof(Rental.StartDate)]),
                                FinalDate = Convert.ToDateTime(dr[nameof(Rental.FinalDate)]),
                                ThingsSoldCounter = Convert.ToInt32(dr[nameof(Rental.ThingsSoldCounter)]),
                                TotalAmountSoldFor = Convert.ToDouble(dr[nameof(Rental.TotalAmountSoldFor)]),
                                Settled = Convert.ToBoolean(dr[nameof(Rental.Settled)]),
                                TenantID = Convert.ToInt32(dr[nameof(Rental.ID)])
                            };
                            _rentals.Add(tempRental);
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

        public Rental GetRental(int id)
        {
            Rental tempRental = _rentals.FirstOrDefault(x => x.ID == id);
            return tempRental != null ? tempRental : throw new Exception("Den valgte rental eksisterer ikke");
        }
    }
}
