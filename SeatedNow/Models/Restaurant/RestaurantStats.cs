﻿namespace SeatedNow.Models
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
        public int RestaurantId { get; set; }

        public int Reservations { get; set; }

        public int Customers { get; set; }

        public int WaitTime { get; set; }

        public double Rating { get; set; }

        public int TotalRatings { get; set; }
    }
}