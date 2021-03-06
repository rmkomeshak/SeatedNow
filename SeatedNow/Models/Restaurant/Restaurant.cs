﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class Restaurant
    {

        public Restaurant(int id, string name, string address, string city, string state, string zipcode, string phoneNumber, string imagePath, bool isVerified, int ownerId, string eventKey, string description, string color, string website, int price)
        {
            Id = id;
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipcode;
            PhoneNumber = phoneNumber;
            ImagePath = imagePath;
            IsVerified = isVerified;
            OwnerId = ownerId;
            EventKey = eventKey;
            Description = description;
            Color = color;
            Website = website;
            Price = price;
        }

        public Restaurant(int id, string name, string address, string city, string state, string zipcode, string phoneNumber, string imagePath, bool isVerified, int ownerId, string eventKey, string description, string color, string keyword1, string keyword2, string keyword3, string website, int price)
        {
            Id = id;
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipcode;
            PhoneNumber = phoneNumber;
            ImagePath = imagePath;
            IsVerified = isVerified;
            OwnerId = ownerId;
            EventKey = eventKey;
            Description = description;
            Color = color;
            Keyword1 = keyword1;
            Keyword2 = keyword2;
            Keyword3 = keyword3;
            Price = price;
            Website = website;

            Tags = new List<string>();
            Tags.Add(Keyword1);
            Tags.Add(Keyword2);
            Tags.Add(Keyword3);
        }

        public Restaurant(string name, string address, string city, string state, string zipcode, string phoneNumber, string imagePath, bool isVerified, int ownerId, string eventKey, string description, string color, string keyword1, string keyword2, string keyword3, string website, int price)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipcode;
            PhoneNumber = phoneNumber;
            ImagePath = imagePath;
            IsVerified = isVerified;
            OwnerId = ownerId;
            EventKey = eventKey;
            Description = description;
            Color = color;
            Keyword1 = keyword1;
            Keyword2 = keyword2;
            Keyword3 = keyword3;
            Price = price;
            Website = website;

            Tags = new List<string>();
            Tags.Add(Keyword1);
            Tags.Add(Keyword2);
            Tags.Add(Keyword3);
        }


        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string State { get; set; }

        public string PhoneNumber { get; set; }

        public string ImagePath { get; set; }

        public bool IsVerified { get; set; }

        public int OwnerId { get; set; }
        
        public string EventKey { get; set; }

        public string Description { get; set; }
        
        public string Color { get; set; }

        public string Website { get; set; }

        public int Price { get; set; }

        public RestaurantStats Stats { get; set; }

        public IEnumerable<RestaurantTableList> Tables { get; set; }

        public List<RestaurantSection> Sections { get; set; }
         
        public List<string> Tags { get; set; }

        public RestaurantHours Hours { get; set; }

        public List<RestaurantRatings> Ratings { get; set; }

        public string Keyword1 { get; set; }

        public string Keyword2 { get; set; }

        public string Keyword3 { get; set; }

    }
}
