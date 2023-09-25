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
    public partial class LockedCabinRepository : ObservableObject, BaseRepositoryInterface
    {
        private ObservableCollection<LockedCabin> _lockedCabins = new();

        public LockedCabinRepository() 
        {

        }

        public void AddLockedCabin()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    LockedCabin lc = new();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO LOCKEDCABIN(DummyColumn)"
                        + " VALUES(0) SELECT SCOPE_IDENTITY()", con);
                    lc.ID = (Convert.ToInt32(cmd.ExecuteScalar()));
                    _lockedCabins.Add(lc);
                    con.Close();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public void RemoveLockedCabin(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE LOCKEDCABIN WHERE ID = @id", con);
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
