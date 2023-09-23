using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using System.IO;






namespace Reolmarkedet.Models
{
    public class SaleRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;

        private List<Sale> saleList = new List<Sale>();

        public SaleRepository() 
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = _configuration.GetConnectionString("MyDBConnection");
        }

        public Sale Add(double totalPrice)
        {
            Sale result;
            try
            {
                if (totalPrice >= 0)
                {
                    result = new Sale()
                    {

                        SoldDate = DateTime.Now,
                        TotalPrice = totalPrice
                    };

                }
                else
                {
                    throw (new ArgumentException("Not all arguemtns are valid"));
                }

                using (SqlConnection con =  new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO SALE(SoldDate, TotalPrice)" 
                        + "VALUES(@SoldDate,@TotalPrice) SELECT SCOPE_IDENTITY()", con);
                    cmd.Parameters.Add("@SoldDate", SqlDbType.DateTime2).Value = result.SoldDate;
                    cmd.Parameters.Add("@TotalPrice", SqlDbType.Float).Value = result.TotalPrice;
                    result.ID = Convert.ToInt32(cmd.ExecuteScalar());
                    this.saleList.Add(result);
                }


            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

            return result;
        }


        //denne metode er ikke testet
        public Sale Get( int id)
        {
            Sale result;
            foreach (Sale s in saleList)
            {
                if (s.ID == id)
                {
                    result = s; break;
                }
            }
       
         return result != null ? result : throw( new ArgumentException("Sale does not exist"));
        }

        public List<Sale> GetAll()
        {
            return saleList;
        }
    }
}
