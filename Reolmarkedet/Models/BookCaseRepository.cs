using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
            bookCase._bookCaseType = bookCaseType;
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO BOOKCASE(BookCaseType)"
                        + "VALUES(@type) SELECT SCOPE_IDENTITY()", con);
                    cmd.Parameters.Add("@type", SqlDbType.Int).Value = (int)bookCase._bookCaseType;
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
                            bookCase._bookCaseType =(BookCaseType)Convert.ToInt32(dr[nameof(BookCaseType)]);
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
    }
}
