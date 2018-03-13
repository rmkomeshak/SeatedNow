using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Managers;
using SeatedNow.Models;
using SeatedNow.Repositories;

namespace SeatedNow.Controllers
{
    public class RestaurantController : Controller
    {
        IRestaurantRepository _restaurantRepository = new RestaurantRepository();
        IStatsRepository _statsRepository = new StatsRepository();
        IReservationRepository _reservationRepository = new ReservationRepository();
        UserSession _userSessionManager = new UserSession();

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Designer()
        {
            return View();
        }

        public IActionResult FloorPlan()
        {
            if (_userSessionManager == null || !_userSessionManager.IsValid())
                return Redirect("~/");
            else if (_userSessionManager.GetRole().Equals("General") && _userSessionManager.IsValid())
                return Redirect("~/Restaurant/List");

            return View(_restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID()));
        }

        public IActionResult Dashboard()
        {
            if (_userSessionManager == null || !_userSessionManager.IsValid())
                return Redirect("~/");
            else if (_userSessionManager.GetRole().Equals("General") && _userSessionManager.IsValid())
                return Redirect("~/Restaurant/List");

            Restaurant restaurant = _restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID());
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(restaurant.Id);
            restaurant.Tables = _restaurantRepository.GetTablesByRestaurantID(restaurant.Id);
            return View(restaurant);

        }

        public IActionResult DashOverview(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Tables = _restaurantRepository.GetTablesByRestaurantID(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashStatistics(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashProfile(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashSettings(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashFloorplan(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);

            return PartialView(restaurant);
        }

        public IActionResult Listing(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Tags = _statsRepository.GetTagsByRestaurantID(Id);
            return View(restaurant);
        }

        public IActionResult List(string SortBy)
        {
            int userId = _userSessionManager.getID();
            ListPage contents;

            switch (SortBy)
            {
                case "reservations":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByReservations(), _restaurantRepository.GetRestaurantsByReservations());
                    ViewBag.SortBy = "Most Reserved Restaurants";
                    break;
                case "ratings":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByRatings(), _restaurantRepository.GetRestaurantsByRatings());
                    ViewBag.SortBy = "Highest Rated Restaurants";
                    break;
                case "waittime":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByWaitTime(), _restaurantRepository.GetRestaurantsByWaitTime());
                    ViewBag.SortBy = "Shortest Wait Time";
                    break;
                default:
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByReservations(), _restaurantRepository.GetRestaurantsByReservations());
                    ViewBag.SortBy = "Most Reserved Restaurants";
                    break;
            }

            return View(contents);
        }

    }
}