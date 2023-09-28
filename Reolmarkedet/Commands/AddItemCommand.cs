using Microsoft.IdentityModel.Tokens;
using Reolmarkedet.Models;
using Reolmarkedet.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reolmarkedet.Commands
{
    public class AddItemCommand : BaseCommand
    {

        private RentalRepository _rentalRepo = new();
        public override bool CanExecute(object? parameter)
        {
            return true;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is SaleViewModels svm) 
            {
                // tjekker på om stregkoden matcher en udljening og om den udlejnings sidste dag er idag eller senere
                if (_rentalRepo.GetRental(svm.BarcodeLabel) != null && _rentalRepo.GetRental(svm.BarcodeLabel).FinalDate >= DateTime.Today)
                {
                    svm.AddSaleItem(svm.BarcodeLabel, svm.PriceLabel);
                    // er en tekst som bliver vidst når en var er tilføjet
                    svm.AddedItem = "Vare tilføjet";
                    


                }
                else
                {
                    // bliver vist hvis ikke stregkoden matchede en udlejning eller matchede en udløbet udlejning
                    svm.AddedItem = "Indtast korrekt stregkode";
                }
            }
        }
    }
}
