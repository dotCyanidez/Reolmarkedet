using Microsoft.IdentityModel.Tokens;
using Reolmarkedet.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reolmarkedet.Commands
{
    public class EndSaleCommand : BaseCommand
    {
        public override bool CanExecute(object? parameter)
        {

            return true;

        }

        public override void Execute(object? parameter)
        {
            if (parameter is SaleViewModels svm) 
            {
                string MessageBoxMessage = svm.MessageBoxText + svm.TotalPriceKr;
                svm.EndSale();
                string MessageBoxCaption = "Salg afsluttet";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxResult result;
                result = MessageBox.Show(MessageBoxMessage, MessageBoxCaption, button);
            }
        }
    }
}
