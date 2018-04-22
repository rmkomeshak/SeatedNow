using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class DashOverview
    {
        public DashOverview(Restaurant r, List<DiningReservation> rl)
        {
            Restaurant = r;
            Reservations = rl;
        }

        public Restaurant Restaurant { get; set; }
        public List<DiningReservation> Reservations { get; set; }
    }
}
