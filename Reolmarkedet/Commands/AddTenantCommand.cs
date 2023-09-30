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
    public class AddTenantCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            string MessageBoxText = "";
            string MessageBoxCaption = "Opretning af lejer";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result;
            if (parameter is TenantViewModel tvm)
                if(tvm.SelectedTenant.ID == 0)
                {
                    if (tvm.SelectedTenant.ContactNr.IsNullOrEmpty() || tvm.SelectedTenant.Email.IsNullOrEmpty())
                    {
                        MessageBoxText = "Udfyld alle felter for at kunne oprette en lejer";
                    }
                    else
                    {
                        tvm.AddTenant(tvm.SelectedTenant);
                        MessageBoxText = "Lejer er blevet oprettet";
                        tvm.UpdateList();
                    }
                }
                else
                {
                    MessageBoxText = "du kan ikke oprette en lejer der allerede eksisterer";
                }

            result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);

        }
    }
}
