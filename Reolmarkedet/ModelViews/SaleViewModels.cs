using CommunityToolkit.Mvvm.ComponentModel;
using Reolmarkedet.Commands;
using Reolmarkedet.Models;
using Reolmarkedet.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Reolmarkedet.ModelViews
{
    public partial class SaleViewModels : BaseViewModels
    {
        private SaleRepository _saleRepository = new();
        public ObservableCollection<SalesItem> Items { get; set; } = new();

        [ObservableProperty]
        private Sale sale;

        [ObservableProperty]
        private int barcodeLabel;

        [ObservableProperty]
        private decimal priceLabel;

        [ObservableProperty]
        private String totalPriceKr ="0Kr.";

        [ObservableProperty]
        private string addedItem = "";

        [ObservableProperty]
        private string messageBoxText = "";

        public ICommand AddItemCommand { get; }
        public ICommand EndSaleCommand { get; }
        public SaleViewModels()
        {
            Title = "Salg";
            Sale = new Sale();
            AddItemCommand = new AddItemCommand();
            EndSaleCommand = new EndSaleCommand();
        }


        public void AddSaleItem(int barcode, decimal price) 
        {
            //Her skal tilføjes noget der tjekker på om der eksistere en udlejning hvor id = barcode
            try
            {
                SalesItem item = new SalesItem(barcode, price);
                Items.Add(item);

                Sale.TotalPrice += item.Price;
                TotalPriceKr = Sale.TotalPrice + "Kr.";
                MessageBoxText += item.ToString() + "\n";
            }
            catch (Exception)
            {

                throw;
            }



        }

        // tilføj så EndSale også tilføjer beløb til de gældende udlejninger og tilføjer til Genstandscounter
        public void EndSale()
        {
            RentalRepository rentalRepo = new RentalRepository();
            foreach (SalesItem item in Items)
            {
                rentalRepo.UpdateRentalToAddSales(item.Barcode, item.Price);
            }
            Items.Clear();
            _saleRepository.Add(Sale.TotalPrice);
            Sale = new Sale();
            TotalPriceKr = Sale.TotalPrice + "Kr.";
            BarcodeLabel = 0;
            PriceLabel = 0;
            AddedItem = "";


        }


    }

    public class SalesItem
    {
        public int Barcode;
        public decimal Price;

        public SalesItem(int barcode, decimal price)
        {
            Barcode = barcode;
            Price = price;
        }

        public override string ToString()
        {
            return $"Stregkode: {Barcode} Pris: {Price}kr.";
        }
    }
}
