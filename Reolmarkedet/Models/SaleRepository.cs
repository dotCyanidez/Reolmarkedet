using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public class SaleRepository
    {

        private List<Sale> saleList = new List<Sale>();

        public SaleRepository() 
        {
            InitializeRepository();
        }

        private void InitializeRepository()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            finally { }
        }

        public Sale Add(int id, DateTime soldDate, double totalPrice)
        {
            Sale result = null;
            try
            {
                if (id > 0 && totalPrice >= 0) 
                { 
                    result = new Sale() 
                    { 
                        ID = id,
                        SoldDate = soldDate,
                        TotalPrice = totalPrice
                    };

                    saleList.Add(result);
                }
                else
                {
                    throw (new ArgumentException("Not all arguemtns are valid"));
                }

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

            return result;
        }

        public Sale Get( int id)
        {
            Sale result = null;
            result = (Sale)saleList.Where(x => x.ID == id);
       
         return result != null ? result : throw( new ArgumentException("Sale does not exist"));
        }

        public List<Sale> GetAll()
        {
            return saleList;
        }
    }
}
