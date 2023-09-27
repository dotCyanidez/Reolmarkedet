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
        private BookCaseRepository _bookCaseRepository = new BookCaseRepository();
        

        // hvis evt reol ID for de gældende reoler skal vises så der kan laves stregkoder til dem
        // så skal AddRental bare laves om fra void til evt List<BookCase> og sendes til viewmodel
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
            List<BookCase> bookCasesForReservation = new List<BookCase>();

            try
            {
                // finder ledige reoler (bookCases) som skal reserveres til den gældende udlejning
                bookCasesForReservation = _bookCaseRepository.FindAvailableBookCases(rental.StartDate, rental.FinalDate, bookCases);
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
                    // reservere de valgte reoler (bookCases)
                    _bookCaseRepository.ReserveBookCases(bookCasesForReservation, rental.ID);
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
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

        public void DeleteRental(int id)
        {
            try
            {
                _bookCaseRepository.DeleteReservationForBookCasesForThisRental(id);
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM RENTAL WHERE ID = @id;", con);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                // skal kun sættes til hvis vi vælger at loade alle rentals inden vi sletter en
                //_rentals.Remove(_rentals.First(x => x.ID == id));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
        }
    }
}
