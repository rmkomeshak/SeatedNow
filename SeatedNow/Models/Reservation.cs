using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class Reservation
    {
        public int ID { get; set; }

        public DateTime Time { get; set; }
        
        public int Guests { get; set; }

        public int OwnerID { get; set; }

        public int RestaurantID { get; set; }

    }
}
