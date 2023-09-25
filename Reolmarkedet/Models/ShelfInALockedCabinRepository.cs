using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public partial class ShelfInALockedCabinRepository : ObservableObject, BaseRepositoryInterface
    {
        private ObservableCollection<ShelfInALockedCabin> _shelfInALockedCabins = new();

        public ShelfInALockedCabinRepository() { }

        public void AddShelfInALockedCabin()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    ShelfInALockedCabin slc = new();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO SHELFINALOCKEDCABIN(DummyColumn)"
                        + " VALUES(0) SELECT SCOPE_IDENTITY()", con);
                    slc.ID = (Convert.ToInt32(cmd.ExecuteScalar()));
                    _shelfInALockedCabins.Add(slc);
                    con.Close();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public void RemoveselfInALockedCabin(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE SHELFINALOCKEDCABIN WHERE ID = @id", con);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
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
