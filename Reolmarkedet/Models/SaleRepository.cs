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
    public class SaleRepository : BaseRepositoryInterface
    {
        //private IConfiguration _configuration;
        //private string _connectionString;

        private List<Sale> _saleList = new List<Sale>();

        public SaleRepository() 
        {
            //_configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //_connectionString = _configuration.GetConnectionString("MyDBConnection");
        }

        public Sale Add(decimal totalPrice)
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


                using (SqlConnection con =  new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO SALE(SoldDate, TotalPrice)" 
                        + "VALUES(@SoldDate,@TotalPrice) SELECT SCOPE_IDENTITY()", con);
                    cmd.Parameters.Add("@SoldDate", SqlDbType.DateTime2).Value = result.SoldDate;
                    cmd.Parameters.Add("@TotalPrice", SqlDbType.Float).Value = result.TotalPrice;
                    result.ID = Convert.ToInt32(cmd.ExecuteScalar());
                    this._saleList.Add(result);
                    con.Close();
                }


            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

            return result;
        }


        //denne metode er ikke testet // men skal heller ikke bruges
        //public Sale Get(int id)
        //{
        //    Sale result = saleList.FirstOrDefault(x => x.ID == id);


        //    return result != null ? result : throw new Exception("Kunne ikke finde det salg du ledte efter");
        //}

        public async void UpdateSale(Sale sale)
        {
            try
            {
                using (SqlConnection con = new(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE SALE SET TotalPrice = @totalPrice WHERE ID = @id", con);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value= sale.ID;
                    cmd.Parameters.Add("@totalPrice", SqlDbType.Float).Value=sale.TotalPrice;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
