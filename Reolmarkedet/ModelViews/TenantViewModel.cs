using CommunityToolkit.Mvvm.ComponentModel;
using Reolmarkedet.Commands;
using Reolmarkedet.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reolmarkedet.ModelViews
{
    public partial class TenantViewModel : BaseViewModels
    {
        private TenantRepository _tenantRepo = new();
        public ObservableCollection<Tenant> Tenants { get; set; } = new();

        [ObservableProperty]
        private Tenant _selectedTenant;

        [ObservableProperty]
        private Tenant _tempNewTenant = new();


        public ICommand AddTenantCommand { get; }
        public ICommand DeleteTenantCommand { get; }
        public ICommand UpdateTenantCommand { get; }

        public TenantViewModel() 
        {
            Title = "Lejer";
            TempNewTenant.Name = "Vælg denne lejer hvis\n du vil oprette en lejer";
            Tenants = _tenantRepo.GetAllTenants();
            Tenants.Insert(0, TempNewTenant);
            SelectedTenant = Tenants.First();
            AddTenantCommand = new AddTenantCommand();
            DeleteTenantCommand = new DeleteTenantCommand();
            UpdateTenantCommand = new UpdateTenantCommand();
           
            
        }

        public void UpdateTenant(Tenant tenant)
        {
            _tenantRepo.UpdateTenant(tenant);
            SelectedTenant = Tenants.First();
            
        }

        public void UpdateList()
        {
            Tenants.Clear();
            Tenants = _tenantRepo.GetAllTenants();
            TempNewTenant = new();
            TempNewTenant.Name = "Vælg denne lejer hvis\n du vil oprette en lejer";
            Tenants.Insert(0, TempNewTenant);
            SelectedTenant = Tenants.First();



        }

        public void AddTenant(Tenant tenant)
        {
            _tenantRepo.AddTenant(tenant.Name,tenant.ContactNr,tenant.Email);
        }

        public void DeleteTenant(Tenant tenant)
        {
            _tenantRepo.RemoveTenant(tenant.ID);
        }

    }
}
