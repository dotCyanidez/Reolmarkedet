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
        public ObservableCollection<Tenant> Tenants { get; set; } = new();

        // en del af partial class RentalVM
        public ObservableCollection<RentalVM> RentalVMs { get; set; } = new();


        [ObservableProperty]
        private List<BookCase> _books;

        //[ObservableProperty]
        //private Rental _selectedRental;

        // en del af RENTALVM
        [ObservableProperty]
        private RentalVM _selectedRentalVM;

        // en del af RENTALVM
        [ObservableProperty]
        private RentalVM _tempRentalVM = new() { Rental = new() { StartDate = DateTime.Now, FinalDate = DateTime.Now } };

        [ObservableProperty]
        private Tenant _selectedTenant;

        //[ObservableProperty]
        //private Rental _tempRental = new() { StartDate = DateTime.Now, FinalDate = DateTime.Now };

        [ObservableProperty]
        private int _clothBookCases = 0;

        [ObservableProperty]
        private int _thingBookCases = 0;

        [ObservableProperty]
        private int _lockedCabinBookCases = 0;

        [ObservableProperty]
        private int _shelfInALockedCabinBookCases = 0;

        public ICommand AddRentalCommand { get;}
        public ICommand DeleteRentalCommand { get;}

        public RentalViewModel()
        {
            Title = "Udlejning";
            Rentals = _rentalRepo.GetAllRentals();
            Tenants = _tenantRepo.GetAllTenants();
            //en del af rentalVM
            RentalVMs.Add(TempRentalVM);
            foreach (Rental rental in Rentals)
            {
                if (rental.TenantID != 0)
                {
                    RentalVMs.Add(new RentalVM() { Rental = rental, Tenant = Tenants.First(x => x.ID == rental.TenantID) });

                }
                else
                {
                    RentalVMs.Add(new RentalVM() { Rental = rental});
                }
            }
            SelectedRentalVM = RentalVMs.First();
            //slut på rentalVM
            //Rentals.Insert(0, TempRental);
            //SelectedRental = Rentals.First();
            AddRentalCommand = new AddRentalCommand();
            DeleteRentalCommand = new DeleteRentalCommand();
        }



        public void UpdateList()
        {
            Rentals.Clear();
            Rentals = _rentalRepo.GetAllRentals();
            RentalVMs.Clear();

            //del af rentalVM
            TempRentalVM = new() { Rental = new() { StartDate = DateTime.Now, FinalDate = DateTime.Now } };
            RentalVMs.Add(TempRentalVM);
            foreach (Rental rental in Rentals)
            {
                if (rental.TenantID != 0)
                {
                    RentalVMs.Add(new RentalVM() { Rental = rental, Tenant = Tenants.First(x => x.ID == rental.TenantID) });

                }
                else
                {
                    RentalVMs.Add(new RentalVM() { Rental = rental });
                }
            }
            SelectedRentalVM = RentalVMs.First();
            ThingBookCases = 0;
            ClothBookCases = 0;
            LockedCabinBookCases = 0;
            ShelfInALockedCabinBookCases = 0;
            //slut på rentalVM
            //TempRental = new() { StartDate = DateTime.Now, FinalDate = DateTime.Now };
            //Rentals.Insert(0, TempRental);
            //SelectedRental = Rentals.First();
        }

        public void AddTenant(Rental rental, List<BookCase> bookCases)
        {
            _rentalRepo.AddRental(rental.StartDate, rental.FinalDate, rental.TenantID, bookCases);
        }

        public void DeleteTenant(Rental rental)
        {
            _rentalRepo.DeleteRental(rental.ID);
        }

        public partial class RentalVM 
        {
            public Rental Rental { get; set; }
            public Tenant Tenant { get; set; }


            public RentalVM AddRentalVM(Rental rental, Tenant tenant)
            {
                RentalVM temp;

                if (rental.TenantID != 0)
                {
                    temp = new() {
                        Rental = rental,
                        Tenant = tenant
                    };
                }
                else
                {
                    temp = new()
                    {
                        Rental = rental
                    };
                }
                return temp;
            }

            public override string ToString()
            {
                string st;
                if (Tenant != null)
                {
                    st = Rental.StartDate.ToString("dd / MM / yyyy") + " til " + Rental.FinalDate.ToString("dd / MM / yyyy")
                                + "\n" + "Lejer: " + Tenant.Name + " " + Tenant.Email;
                }
                else
                {
                    st = Rental.StartDate.ToString("dd / MM / yyyy") + " til " + Rental.FinalDate.ToString("dd / MM / yyyy");
                }

                return st;
            }
        }
    }
}
