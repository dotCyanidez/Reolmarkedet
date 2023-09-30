using Reolmarkedet.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reolmarkedet.Commands
{
    internal class UpdateTenantCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter is TenantViewModel tvm)
            {
                
                string MessageBoxText = $"\n Navn: {tvm.SelectedTenant.Name}"
                    + $"\n Email: {tvm.SelectedTenant.Email}\n Tlf: {tvm.SelectedTenant.ContactNr}";
                string MessageBoxCaption = "er du sikker på du vil opdatere lejer til:";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxResult result;
                result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);
                if (result == MessageBoxResult.Yes)
                    tvm.UpdateTenant(tvm.SelectedTenant);
                else if(result == MessageBoxResult.No)
                    tvm.UpdateList();

            };
        }
    }
}
