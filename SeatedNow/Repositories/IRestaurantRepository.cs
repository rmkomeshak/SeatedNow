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
        List<RestaurantListViewModel> GetRestaurantsByName(string name);
        List<RestaurantListViewModel> GetRestaurantsByReservations();
        List<RestaurantListViewModel> GetRestaurantsByTags(List<string> tags);
        List<RestaurantListViewModel> GetRestaurantsByRatings();
        List<RestaurantListViewModel> GetRestaurantsByWaitTime();
        List<SideAdminRestaurantListViewModel> GetRestaurantsAdminList();
        Restaurant GetRestaurantByID(int id);
        Restaurant GetRestaurantByOwnerID(int id);
        List<RestaurantTableList> GetTablesByRestaurantID(int id);
        bool UpdateRestaurantTable(RestaurantTableList t);
        RestaurantListViewModel GetRestaurantListViewModelByID(int id);
        Restaurant GetRestaurantByAddress(string address, string city, string state, string zipcode);
        Restaurant GetrestaurantByPhone(string phone);
        bool UpdateRestaurant(Restaurant restaurant);
        Boolean IsRestaurantRegistered(string email);
        Boolean InputRating(int restaurant_id, float rating, string review, DateTime time, int userId);
        bool UpdateTags(Restaurant restaurant);
        bool UpdateSections(int restaurant_id, string section1, string section2, string section3, string section4, string section5, string section6, List<RestaurantTableList> tables);
        List<RestaurantSection> GetSections(int restaurant_id);
        bool OccupyTable(int restaurant_id, int table_id);
        bool FreeTable(int restaurant_id, int table_id);
        bool CreateMetricsTable(string name);
    }
}
