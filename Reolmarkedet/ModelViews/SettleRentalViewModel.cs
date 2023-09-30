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
    public partial class SettleRentalViewModel : BaseViewModels
    {
        private RentalRepository _rentalRepo = new();
        private TenantRepository _tenantRepo = new();

        public ObservableCollection<Rental> Rentals { get; set; } = new();
        public ObservableCollection<Tenant> Tenants { get; set; } = new();
        public ObservableCollection<SettleRentalVM> settleRentalVMs { get; set; } = new();
        public ObservableCollection<SettleRentalVM> SettledRentalVMs { get; set; } = new();

        [ObservableProperty]
        private SettleRentalVM _selectedSettleRentalVM;

        public ICommand SettleCommand { get; set; }
        public SettleRentalViewModel()
        {
            Rentals = _rentalRepo.GetAllRentals();
            Tenants = _tenantRepo.GetAllTenants();
            SettleCommand = new SettleCommand();
            foreach (Rental rental in Rentals)
            {
                if (!rental.Settled && rental.FinalDate < DateTime.Now)
                        settleRentalVMs.Add(new() { Rental = rental, Tenant = Tenants.First(x => x.ID == rental.TenantID) });
            }
        }

        public void Settle(Rental rental)
        {
            _rentalRepo.SettleRental(rental.ID);
        }
    }

    public partial class SettleRentalVM
    {
        public Rental Rental { get; set; }
        public Tenant Tenant { get; set; }

        public SettleRentalVM Add(Rental rental, Tenant tenant)
        {
            SettleRentalVM temp = new();
            temp.Rental = rental;
            temp.Tenant = tenant;
            return temp;

        }
    }
}
