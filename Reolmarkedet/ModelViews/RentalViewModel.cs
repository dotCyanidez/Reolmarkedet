using CommunityToolkit.Mvvm.ComponentModel;
using Reolmarkedet.Commands;
using Reolmarkedet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reolmarkedet.ModelViews
{
    public partial class RentalViewModel : BaseViewModels
    {
        private RentalRepository _rentalRepo = new();
        private TenantRepository _tenantRepo = new();
        public ObservableCollection<Rental> Rentals { get; set; } = new();
        public ObservableCollection<Tenant> Tenants { get; set; }
        [ObservableProperty]
        private List<BookCase> _books;

        [ObservableProperty]
        private Rental _selectedRental;

        [ObservableProperty]
        private Tenant _selectedTenant;

        [ObservableProperty]
        private Rental _tempRental = new() { StartDate = DateTime.Now, FinalDate = DateTime.Now };

        [ObservableProperty]
        private int _clothBookCases;

        [ObservableProperty]
        private int _ThingBookCases;

        [ObservableProperty]
        private int _LockedCabinBookCases;

        [ObservableProperty]
        private int _ShelfInALockedCabinBookCases;

        public ICommand AddRentalCommand { get;}
        public ICommand DeleteRentalCommand { get;}

        public RentalViewModel()
        {
            Title = "Udlejning";
            Rentals = _rentalRepo.GetAllRentals();
            Tenants = _tenantRepo.GetAllTenants();
            Rentals.Insert(0, TempRental);
            SelectedRental = Rentals.First();
            AddRentalCommand = new AddRentalCommand();
            DeleteRentalCommand = new DeleteRentalCommand();
        }



        public void UpdateList()
        {
            Rentals.Clear();
            Rentals = _rentalRepo.GetAllRentals();
            TempRental = new() { StartDate = DateTime.Now, FinalDate = DateTime.Now };
            Rentals.Insert(0, TempRental);
            SelectedRental = Rentals.First();
        }

        public void AddTenant(Rental rental, List<BookCase> bookCases)
        {
            _rentalRepo.AddRental(rental.StartDate, rental.FinalDate, rental.TenantID, bookCases);
        }

        public void DeleteTenant(Rental rental)
        {
            _rentalRepo.DeleteRental(rental.ID);
        }
    }
}
