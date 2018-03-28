using System;
using System.ComponentModel.DataAnnotations;

namespace SeatedNow.Models
{
    public class RestaurantRatings
    {

        public RestaurantRatings(double rating, string comment)
        {
            Rating = rating;
            Comment = comment;
        }

        [Range(1, 5, ErrorMessage = "Woops, please select your rating again!")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "Please enter a comment")]
        public string Comment { get; set; }
    }
}
