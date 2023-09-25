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
        private int barcodeLabel = 0120;

        [ObservableProperty]
        private double priceLabel = 12.4;

        public ICommand AddItemCommand { get; }
        public ICommand EndSaleCommand { get; }
        public SaleViewModels()
        {
            Title = "Salg";
            Sale = _saleRepository.Add(0);
            AddItemCommand = new AddItemCommand();
            EndSaleCommand = new EndSaleCommand();
        }


        public void AddSaleItem(int barcode, double price) 
        {
            //Her skal tilføjes noget der tjekker på om der eksistere en udlejning hvor id = barcode
            try
            {
                SalesItem item = new SalesItem(barcode, price);
                Items.Add(item);

                Sale.TotalPrice += item.Price;

            }
            catch (Exception)
            {

                throw;
            }

        }

        // tilføj så EndSale også tilføjer beløb til de gældende udlejninger og tilføjer til Genstandscounter
        public void EndSale()
        {
            Items.Clear();
            _saleRepository.UpdateSale(Sale);

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

        public override string ToString()
        {
            return $"Stregkode: {Barcode} Pris: {Price}kr.";
        }
    }
}
