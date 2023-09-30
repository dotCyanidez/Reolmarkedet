using Reolmarkedet.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Commands
{
    public class AddRentalCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter is RentalViewModel rvm)
            {

            };
        }
    }
}
