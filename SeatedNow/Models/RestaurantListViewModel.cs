using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class RestaurantListViewModel
    {

        public RestaurantListViewModel(string name, string address, string city, string state, string zipcode, string imagePath)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipcode;
            ImagePath = imagePath;
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }

        public string ImagePath { get; set; }
    }
}
