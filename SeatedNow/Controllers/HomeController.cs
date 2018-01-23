using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Models;

namespace SeatedNow.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "All about SeatedNow.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact the SeatedNow team.";

            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Sign into your account.";

            return View();
        }

        public IActionResult Register()
        {
            ViewData["Message"] = "Register for an account.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
