using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class ListPage
    {
        public ListPage(List<DiningReservation> dr, List<RestaurantListViewModel> rl, List<RestaurantListViewModel> r)
        {
            UserReservations = dr;
            RestaurantList = rl;
            Restaurants = r;
        }

        public List<DiningReservation> UserReservations { get; set; }
        public List<RestaurantListViewModel> RestaurantList { get; set; }
        public List<RestaurantListViewModel> Restaurants { get; set; }
    }
}
