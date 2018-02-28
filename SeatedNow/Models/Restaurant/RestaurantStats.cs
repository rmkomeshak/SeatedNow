namespace SeatedNow.Models
{
    public class RestaurantStats
    {
        public RestaurantStats(int id, int reservations, int customers, int waittime, int restaurant_id)
        {
            Id = id;
            Reservations = reservations;
            Customers = customers;
            WaitTime = waittime;
            RestaurantId = restaurant_id;
        }

        public int Id { get; set; }

        public int Reservations { get; set; }

        public int Customers { get; set; }

        public int WaitTime { get; set; }

        public int RestaurantId { get; set; }
    }
}