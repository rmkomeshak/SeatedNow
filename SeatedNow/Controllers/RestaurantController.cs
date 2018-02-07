using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeatedNow.Controllers
{
    public class RestaurantController : Controller
    {
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
    }
}