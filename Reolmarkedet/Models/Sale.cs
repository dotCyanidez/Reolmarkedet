using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public class Sale
    {
        public int ID { get; set; }
        public DateTime SoldDate { get; set; }
        public double TotalPrice { get; set; }

    }
}
