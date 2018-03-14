using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class RestaurantRatings
    {

        public RestaurantRatings(double rating, string comment)
        {
            Rating = rating;
            Comment = comment;
        }

        public double Rating { get; set; }

        public string Comment { get; set; }
    }
}
