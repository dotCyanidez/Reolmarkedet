using Reolmarkedet.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Reolmarkedet.Commands
{
    public class AddRentalCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            string MessageBoxText = "";
            string MessageBoxCaption = "Opretning af udlejning";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result;
            TimeSpan ts = new TimeSpan(5, 0, 0, 0);
            Boolean succes = false;
            if (parameter is RentalViewModel rvm)
            {
                // til at tjekke at rental minimum har 5 dage mellem start og slut dato
                DateTime dt = rvm.SelectedRentalVM.Rental.StartDate + ts;
                // hvis tenant ikke er en gyldig tenant
                if (rvm.SelectedRentalVM.Tenant == null || rvm.SelectedRentalVM.Tenant.ID == 0)
                {
                    MessageBoxText = "Du skal vælge en lejer som du vil oprette udlejningen til";
                    result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);

                    return;
                }
            
                if (rvm.SelectedRentalVM.Rental.ID != 0)
                {
                    MessageBoxText = "Du kan ikke oprette en allerede eksisterende udlejning";
                    result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);

                    return;

                }

                // hvis der ikke er mere end 1 reol
                if (rvm.ClothBookCases + rvm.ThingBookCases + rvm.LockedCabinBookCases + rvm.ShelfInALockedCabinBookCases <= 0)
                {
                    MessageBoxText = " vælg antal af reoler / aflåste skabe / hylder som skal lejes";
                    result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);

                    return;

                }
                // hvis start dato ikke er før igår og slut dato ikke er mere end 5 dage efter start dato
                if (rvm.SelectedRentalVM.Rental.StartDate <= DateTime.Now.AddDays(-1) || rvm.SelectedRentalVM.Rental.FinalDate <= dt)
                {
                    MessageBoxText = "Vælg nogle gyldige start og slut datoer";
                    result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);

                    return;
                }

                rvm.SelectedRentalVM.Rental.TenantID = rvm.SelectedRentalVM.Tenant.ID;
                succes = rvm.AddRental(rvm.SelectedRentalVM.Rental, rvm.ThingBookCases, rvm.ClothBookCases, rvm.LockedCabinBookCases, rvm.ShelfInALockedCabinBookCases);
                MessageBoxText = "Du har oprettet Udlejningen: " + "Start dato: " + rvm.SelectedRentalVM.Rental.StartDate
                    + " Slut dato: " + rvm.SelectedRentalVM.Rental.FinalDate + "\n Tilhørende lejer: " + rvm.SelectedRentalVM.Tenant.Name;
                rvm.UpdateList();
            }

            if(succes)
            result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);

        }
    }
}
