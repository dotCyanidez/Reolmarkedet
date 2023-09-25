using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public interface BaseRepositoryInterface
    {
        protected static IConfiguration _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        protected static string _connectionString = _configuration.GetConnectionString("MyDBConnection");
    }
}
