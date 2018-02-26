using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Repositories;

namespace SeatedNow.Controllers
{
    public class RestaurantController : Controller
    {
        IRestaurantRepository _restaurantRepository = new RestaurantRepository();

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