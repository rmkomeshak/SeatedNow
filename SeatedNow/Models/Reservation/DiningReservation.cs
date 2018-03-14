using System;

namespace SeatedNow.Models
{
    public class DiningReservation
    {

        public DiningReservation(int reservation_id, int restaurant_id, int owner_id, int guests, DateTime time, int table_id, string section)
        {
            ReservationID = reservation_id;
            RestaurantID = restaurant_id;
            OwnerID = owner_id;
            Guests = guests;
            Time = time;
            TableID = table_id;
            Section = section;
        }

        public DiningReservation(int restaurant_id, int owner_id, int guests, DateTime time, int table_id, string section)
        {
            RestaurantID = restaurant_id;
            OwnerID = owner_id;
            Guests = guests;
            Time = time;
            TableID = table_id;
            Section = section;
        }

        public DiningReservation()
        {
            Time = new DateTime(1990, 1, 1);
        }

        public int ReservationID { get; set; }

        public int RestaurantID { get; set; }

        public int OwnerID { get; set; }

        public int Guests { get; set; }

        public DateTime Time { get; set; }
        
        public int TableID { get; set; }

        public string Section { get; set; }

    }
}
