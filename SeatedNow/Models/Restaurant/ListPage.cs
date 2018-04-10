using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class ListPage
    {
        public ListPage(List<DiningReservation> dr, List<RestaurantListViewModel> rl)
        {
            UserReservations = dr;
            RestaurantList = rl;
        }

        public List<DiningReservation> UserReservations { get; set; }
        public List<RestaurantListViewModel> RestaurantList { get; set; }
    }
}
