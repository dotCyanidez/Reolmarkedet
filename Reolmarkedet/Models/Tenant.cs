using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public class Tenant
    {
        public int ID { get; set; }
        public String? Name { get; set; }
        public String? ContactNr { get; set; }
        public String? Email { get; set; }

        //public Tenant(string name, string contactNr, string email) 
        //{
        //    Name = name;
        //    ContactNr = contactNr;
        //    Email = email;
        //}

    }
}
