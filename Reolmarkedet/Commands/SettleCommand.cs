using Reolmarkedet.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reolmarkedet.Commands
{
    public class SettleCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            string MessageBoxText = "";
            string MessageBoxCaption = "Afregning af udlejning";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result;
            decimal ButikProcent = 0.10m;
            if (parameter is SettleRentalViewModel srvm)
            {
                if (srvm.SelectedSettleRentalVM.Rental == null)
                {
                    MessageBoxText = "Vælg en udlejning";
                    result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);
                    return;
                }

                srvm.Settle(srvm.SelectedSettleRentalVM.Rental);
                MessageBoxText = "Afregn udlejning: " + "\n" + "Slut dato: " + srvm.SelectedSettleRentalVM.Rental.FinalDate 
                    + "For Lejer: " + srvm.SelectedSettleRentalVM.Tenant.Name + " Tlf: " + srvm.SelectedSettleRentalVM.Tenant.ContactNr 
                    +"Email: " + srvm.SelectedSettleRentalVM.Tenant.Email + "\n"
                    + "Antal genstande solgt: " + srvm.SelectedSettleRentalVM.Rental.ThingsSoldCounter + "\n"
                    + "Beløb til Lejer: " + (Convert.ToDecimal(srvm.SelectedSettleRentalVM.Rental.TotalAmountSoldFor) * (1 - ButikProcent))
                    + " beløbet er fratrukket Reolmarkedets " + (ButikProcent * 100) + "%";
                result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);
                srvm.SelectedSettleRentalVM.Rental.TotalAmountSoldFor = Convert.ToDouble(Convert.ToDecimal(srvm.SelectedSettleRentalVM.Rental.TotalAmountSoldFor) * (1 - ButikProcent));
                srvm.SettledRentalVMs.Add(srvm.SelectedSettleRentalVM);

                srvm.settleRentalVMs.Remove(srvm.SelectedSettleRentalVM);
            }
        }
    }
}
