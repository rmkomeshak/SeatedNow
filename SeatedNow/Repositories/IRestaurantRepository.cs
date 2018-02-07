using SeatedNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    interface IRestaurantRepository
    {
        void RegisterNewRestaurant(Restaurant restaurant);
        void DeleteRestaurant(Restaurant restaurant);
        List<RestaurantListViewModel> GetRestaurants();
        Restaurant GetRestaurantByID(int id);
        Restaurant GetRestaurantByAddress(string address, string city, string state, string zipcode);
        Restaurant GetrestaurantByPhone(string phone);
        Boolean IsRestaurantRegistered(string email);
    }
}
