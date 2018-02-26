using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SideAdminRestaurantListViewModel
{

    public SideAdminRestaurantListViewModel(int id, string name, bool isVerified)
    {
        Id = id;
        Name = name;
        IsVerified = isVerified;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsVerified { get; set; }

}
