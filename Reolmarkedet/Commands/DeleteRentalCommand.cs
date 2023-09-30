using Reolmarkedet.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reolmarkedet.Commands
{
    public class DeleteRentalCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            string messageText = "Vælg en gyldig udlejning";
            string messageCaption = "Udlejning";
            MessageBoxButton buttonOk = MessageBoxButton.OK;
            MessageBoxButton buttonYN = MessageBoxButton.YesNo;
            MessageBoxResult result;
            if (parameter is RentalViewModel rvm)
            {

                if (rvm.SelectedRentalVM.Rental.ID == 0)
                {
                    result = MessageBox.Show(messageText, messageCaption, buttonOk);
                    return;
                }


                messageCaption = "Er du sikker på du vil slette udlejning: ";
                messageText = "Start dato: " + rvm.SelectedRentalVM.Rental + "\n Slut dato: " + rvm.SelectedRentalVM.Rental.FinalDate
                    + "Tilhørende lejer: " + rvm.SelectedRentalVM.Tenant.Name;
                result = MessageBox.Show(messageText, messageCaption, buttonYN);

                if (result == MessageBoxResult.Yes)
                {
                    rvm.DeleteRental(rvm.SelectedRentalVM.Rental);
                    rvm.UpdateList();
                }

            }
        }
    }
}
