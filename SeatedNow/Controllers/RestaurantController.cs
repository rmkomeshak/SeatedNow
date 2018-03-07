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
            return View(restaurant);

        }

        public IActionResult DashOverview()
        {
            return PartialView();
        }

        public IActionResult DashStatistics()
        {
            return PartialView();
        }

        public IActionResult test()
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID());
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(restaurant.Id);

            return View(restaurant);
        }

        public IActionResult Generic()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(_restaurantRepository.GetRestaurants());
        }

    }
}