using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public partial class Sale : ObservableObject
    {
        public int ID { get; set; }
        public DateTime SoldDate { get; set; }
        [ObservableProperty]
        private double _totalPrice;

    }
}
