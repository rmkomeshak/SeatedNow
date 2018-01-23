using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Models;
using SeatedNow.Repositories;

namespace SeatedNow.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult CreateUser(String FirstName, String LastName, String Email, String PhoneNumber, String Password)
        {
            UsersRepository userRepo = new UsersRepository();
            UserAccount userAccount = new UserAccount(FirstName, LastName, Email, PhoneNumber, Password);

            userRepo.CreateUser(userAccount);
            return Content(FirstName + " | " + LastName + " | " + Email + " | " + Password );
        }
    }
}