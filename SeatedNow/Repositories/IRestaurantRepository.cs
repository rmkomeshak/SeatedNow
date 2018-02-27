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
        void DeleteRestaurant(int id);
        List<RestaurantListViewModel> GetRestaurants();
        List<SideAdminRestaurantListViewModel> GetRestaurantsAdminList();
        Restaurant GetRestaurantByID(int id);
        Restaurant GetRestaurantByOwnerID(int id);
        Restaurant GetRestaurantByAddress(string address, string city, string state, string zipcode);
        Restaurant GetrestaurantByPhone(string phone);
        bool UpdateRestaurant(Restaurant restaurant);
        Boolean IsRestaurantRegistered(string email);
    }
}
