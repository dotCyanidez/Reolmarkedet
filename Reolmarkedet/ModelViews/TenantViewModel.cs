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
    public partial class TenantViewModel : BaseViewModels
    {
        private TenantRepository _tenantRepo = new();
        public ObservableCollection<Tenant> Tenants { get; set; } = new();

        public TenantViewModel() 
        {
            Tenants = _tenantRepo.GetAllTenants();
           
            
        }

    }
}
