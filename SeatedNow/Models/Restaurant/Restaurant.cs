using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class Restaurant
    {
        public Restaurant(int id, string name, string address, string city, string state, string zipcode, string phoneNumber, string imagePath)
        {
            Id = id;
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipcode;
            PhoneNumber = phoneNumber;
            ImagePath = ImagePath;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string State { get; set; }

        public string PhoneNumber { get; set; }

        public string ImagePath { get; set; }
    }
}
