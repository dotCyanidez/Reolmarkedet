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
        public override void Execute(object? parameter)
        {
            if (parameter is SaleViewModels svm) 
            {
                svm.AddSaleItem(svm.BarcodeLabel, svm.PriceLabel);
            }
        }
    }
}
