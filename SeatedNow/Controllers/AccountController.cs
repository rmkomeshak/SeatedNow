using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
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
            String HashedPassword = GenerateHash(Password);
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

        public JsonResult EmailIsRegistered(string Email)
        {
            UsersRepository userRepo = new UsersRepository();
            if (!userRepo.IsEmailRegistered(Email))
            {
                return Json(true);
            }

            return Json(false);
        }

        public IActionResult LoginUser(String Email, String Password)
        {
            UsersRepository userRepo = new UsersRepository();
            string HashedPassword = GenerateHash(Password);

            if (PasswordsMatch(userRepo.GetHashedPassword(Email), HashedPassword))
            {
                UserAccount account = userRepo.GetUserByEmail(Email);
                return Content(account.Name);
            } else
            {
                return Content("Failure!");
            }
        }
        
        public IActionResult LogoutUser()
        {
            return View("../Home/Index");
        }

        private string GenerateHash(string password)
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
            return (savedPass.Equals(inputPass));
        }

    }
}