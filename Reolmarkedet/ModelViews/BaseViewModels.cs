using CommunityToolkit.Mvvm.ComponentModel;
using Reolmarkedet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.ModelViews
{
    public partial class BaseViewModels : ObservableObject
    {
        //til test af rental repo
        private RentalRepository _rentalrepo = new();
        //private ObservableCollection<BookCase> bookCases = new ObservableCollection<BookCase>()
        //{
        //    new BookCase() { BookCaseType = BookCaseType.ThingsBookCase },
        //    new BookCase() { BookCaseType = BookCaseType.ClothBookCase },
        //    new BookCase() { BookCaseType = BookCaseType.LockedCabin }
        //};
        //private DateTime dt = new DateTime(2023, 10, 16);

        public BaseViewModels() 
        {

             //til test af rental repo
            //_rentalrepo.AddRental(DateTime.Now, this.dt, 1, bookCases.ToList());
            // til test af DeleteRental Metoden
            //_rentalrepo.DeleteRental(8);
            Title = "ReolMarkedet";
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        bool IsNotBusy => !IsBusy;




    }
}
