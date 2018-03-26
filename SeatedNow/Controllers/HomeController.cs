using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Models;
using SeatedNow.Managers;

namespace SeatedNow.Controllers
{
    public class HomeController : Controller
    {
        UserSession _userSessionManager = new UserSession();
        public IActionResult Index()
        {
            if (_userSessionManager == null || _userSessionManager.IsValid())
                return Redirect("~/Restaurant/List");

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult TestGoogle()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
