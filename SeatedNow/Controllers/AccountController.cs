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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult RegisterNewUser(String Name, String Email, String PhoneNumber, String Password)
        {
            UsersRepository userRepo = new UsersRepository();
            UserAccount userAccount = new UserAccount(Name, Email, PhoneNumber, Password);

            userRepo.CreateUser(userAccount);

            return Content("You have successfully registered! (This wont be the page registration goes to, we have yet to implement it");
        }

        public IActionResult LoginUser(String Email, String Password)
        {
            UserAccount account = new UserAccount("Dane Mazzaro", Email, "2484960964", Password);
            if (Email == "damazzaro@oakland.edu" && Password == "hello123")
            {
                return Content("Hello");
            }
            return Content(Email + " | " + Password);
        }
        
        public IActionResult LogoutUser()
        {
            return View("../Home/Index");
        }

    }
}