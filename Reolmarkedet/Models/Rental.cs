using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public class Rental
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int ThingsSoldCounter { get; set; }
        public double TotalAmountSoldFor { get; set; }
        public Boolean Settled { get; set; }
        public int TenantID { get; set; }

        public override string ToString()
        {
            return $"{StartDate} --- {FinalDate}";
        }
    }
}
