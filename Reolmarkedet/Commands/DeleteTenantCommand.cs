using Reolmarkedet.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reolmarkedet.Commands
{
    internal class DeleteTenantCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            string MessageBoxText = "";
            string MessageBoxCaption = "Slet lejer";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result;
            if (parameter is TenantViewModel tvm)
            {
                if (tvm.SelectedTenant.ID != 0)
                {
                    tvm.DeleteTenant(tvm.SelectedTenant);
                    tvm.UpdateList();
                    MessageBoxText = "Lejer slettet";
                }
                else
                {
                    MessageBoxText = "Vælg en lejer du vil slette";
                }

                result = MessageBox.Show(MessageBoxText, MessageBoxCaption, button);
            }
        }
    }
}
