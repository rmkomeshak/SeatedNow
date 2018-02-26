using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models.SiteAdmin
{
    public class SiteAdminAccountListViewModel
    {

        public SiteAdminAccountListViewModel(int id, string name, string role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public int Id { get; set; }

        public String Name { get; set; }

        public String Role { get; set; }
    }
}
