using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class Listing
    {
        public Listing(Restaurant r, DiningReservation dr)
        {
            Restaurant = r;
            Reservation = dr;
        }

        public DiningReservation Reservation { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
