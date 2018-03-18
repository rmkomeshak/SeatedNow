using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class DashStats
    {
        public DashStats(Restaurant r, List<RestaurantListViewModel> rl)
        {
            Restaurant = r;
            OtherRestaurants = rl;
        }

        public Restaurant Restaurant { get; set; }
        public List<RestaurantListViewModel> OtherRestaurants { get; set; }
    }
}
