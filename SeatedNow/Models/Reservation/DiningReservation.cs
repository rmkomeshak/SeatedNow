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

        public DiningReservation(int restaurant_id, int owner_id, int guests, DateTime time, int table_id, string section, bool inuse)
        {
            RestaurantID = restaurant_id;
            OwnerID = owner_id;
            Guests = guests;
            Time = time;
            TableID = table_id;
            Section = section;
            InUse = inuse;
        }

        public DiningReservation(int reservation_id, int restaurant_id, int owner_id, int guests, DateTime time, int table_id, string section, bool inuse)
        {
            ReservationID = reservation_id;
            RestaurantID = restaurant_id;
            OwnerID = owner_id;
            Guests = guests;
            Time = time;
            TableID = table_id;
            Section = section;
            InUse = inuse;
        }

        public DiningReservation(string name, string table_name, int guests)
        {
            Name = name;
            TableName = table_name;
            Guests = guests;
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

        public bool InUse { get; set; }

        public string TableName { get; set; }
        
        public string Name { get; set; }

    }
}
