using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.ModelViews
{
    public partial class BaseViewModels : ObservableObject
    {
        public BaseViewModels() 
        {
            Title = "ReolMarkedet";
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        bool IsNotBusy => !isBusy;




    }
}
