using CommunityToolkit.Mvvm.ComponentModel;
using Reolmarkedet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.ModelViews
{
    public partial class BookcaseViewModel : BaseViewModels
    {
        private BookCaseRepository _bookCaseRepo = new();

        public BookcaseViewModel()
        {
            _bookCaseRepo.GetAllBookCases();

        }
    }
}
