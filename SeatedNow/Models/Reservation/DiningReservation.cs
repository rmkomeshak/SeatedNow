using System;

namespace SeatedNow.Models
{
    public class DiningReservation
    {

        public int ID { get; set; }

        public DateTime Time { get; set; }
        
        public int Guests { get; set; }

        public int OwnerID { get; set; }

        public int RestaurantID { get; set; }

    }
}
