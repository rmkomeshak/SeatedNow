using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            String HashedPassword = HashPassword(Password);
            UserAccount userAccount = new UserAccount(Name, Email, PhoneNumber, HashedPassword);

            if (userRepo.IsEmailRegistered(userAccount.Email))
            {
                return Content("Error this email is already registered");
            }
            else
            {
                userRepo.RegisterNewUser(userAccount);
                return Content("You have successfully registered!");
            }
        }

        public IActionResult LoginUser(String Email, String Password)
        {
            return Content(Email + " | " + Password);
        }
        
        public IActionResult LogoutUser()
        {
            return View("../Home/Index");
        }

        private string HashPassword(string password)
        {
            SHA1 sha1 = SHA1.Create();
            StringBuilder hashPass = new StringBuilder();

            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(password));

            for (int i = 0; i < hashData.Length; i++)
            {
                hashPass.Append(hashData[i].ToString());
            }

            return hashPass.ToString();
        }

        private Boolean PasswordsMatch(string savedPass, string inputPass)
        {
            string hashedInputPass = HashPassword(inputPass);

            if (String.Equals(savedPass, hashedInputPass))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}