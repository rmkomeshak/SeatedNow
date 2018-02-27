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
            return View();
        }

        public IActionResult Dashboard()
        {
            if (!_userSessionManager.IsValid() || _userSessionManager.GetRole().Equals("General"))
                return Redirect("~/");

            return View(_restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID()));

        }

        public IActionResult DashOverview()
        {
            return PartialView();
        }

        public IActionResult DashStatistics()
        {
            return PartialView();
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