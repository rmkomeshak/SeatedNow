namespace SeatedNow.Models
{
    public class RestaurantStats
    {
        public RestaurantStats(int restaurant_id, int reservations, int customers, int waittime, double rating, int totalRatings)
        {
            RestaurantId = restaurant_id;
            Reservations = reservations;
            Customers = customers;
            WaitTime = waittime;
            Rating = rating;
            TotalRatings = totalRatings;
        }

        public RestaurantStats(int restaurant_id, int reservations, int customers, int waittime, double rating, int totalRatings, int pageViews)
        {
            RestaurantId = restaurant_id;
            Reservations = reservations;
            Customers = customers;
            WaitTime = waittime;
            Rating = rating;
            TotalRatings = totalRatings;
            PageViews = pageViews;
        }

        public int RestaurantId { get; set; }

        public int Reservations { get; set; }

        public int Customers { get; set; }

        public int WaitTime { get; set; }

        public double Rating { get; set; }

        public int TotalRatings { get; set; }

        public int PageViews { get; set; }
    }
}