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

            return View("../Home/Index");
        }

        public IActionResult LoginUser(String Email, String Password)
        {
            Console.WriteLine("HELLO " + Email + " " + Password);
            if (Email == "damazzaro@oakland.edu" && Password == "hello123")
            {
                return View("../Home/IndexL");
            }
            return View("../Account/Login");
        }
        
        public IActionResult LogoutUser()
        {
            return View("../Home/Index");
        }

    }
}