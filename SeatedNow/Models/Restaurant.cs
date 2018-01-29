using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class Restaurant
    {
        public RestaurantAccount(string name, string address, string city, string zip, string state , string phoneNumber)
        {
            Name = name;
            Address = address;
            City = city;
            Zip = zip;
            State = state;
            PhoneNumber = phoneNumber;
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public string State { get; set; }

        public string PhoneNumber { get; set; }



    }
}
