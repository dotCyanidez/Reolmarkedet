using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
