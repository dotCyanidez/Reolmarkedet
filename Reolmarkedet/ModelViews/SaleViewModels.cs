using CommunityToolkit.Mvvm.ComponentModel;
using Reolmarkedet.Models;
using Reolmarkedet.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reolmarkedet.ModelViews
{
    public partial class SaleViewModels : BaseViewModels
    {
        private SaleRepository _saleRepository = new();
        public ObservableCollection<SalesItem> Items = new();

        [ObservableProperty]
        private Sale sale;

        [ObservableProperty]
        private int barcodeLabel = 0120;

        [ObservableProperty]
        private double priceLabel = 12.4;
        public SaleViewModels()
        {
            Title = "Salg";
            Sale = _saleRepository.Add(0);
        }


        public void AddSaleItem(int barcode,double price) 
        {
            //Her skal tilføjes noget der tjekker på om der eksistere en udlejning hvor id = barcode
            SalesItem item = new SalesItem( barcode, price);
            Items.Add(item);

            Sale.TotalPrice += item.Price;

        }


    }

    public class SalesItem
    {
        public int Barcode;
        public double Price;

        public SalesItem(int barcode, double price)
        {
            Barcode = barcode;
            Price = price;
        }
    }
}
