using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Managers;
using SeatedNow.Models;
using SeatedNow.Repositories;

namespace SeatedNow.Controllers
{
    public class AccountController : Controller
    {

        UserSession _userSessionManager = new UserSession();
        IUserRepository _userRepository = new UserRepository();
        IRestaurantRepository _restaurantRepository = new RestaurantRepository();

        public IActionResult Index()
        {
            return View(_restaurantRepository.GetRestaurants());
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View(_userRepository.GetUserByEmail(HttpContext.Session.GetString("_email")));
        }

        public IActionResult RegisterUser(String Name, String Email, String PhoneNumber, String Password)
        {
            if (_userRepository.IsEmailRegistered(Email))
            {
                return Content("Error this email is already registered");
            }
            else
            {
                String HashedPassword = GenerateHash(Password);
                UserAccount userAccount = new UserAccount(Name, Email, PhoneNumber, HashedPassword);

                _userRepository.RegisterNewUser(userAccount);
                return Redirect("Login");
            }
        }

        public IActionResult LoginUser(String Email, String Password)
        {
            if (ModelState.IsValid && !String.IsNullOrEmpty(Password))
            {
                string HashedPassword = GenerateHash(Password);

                if (EmailExists(Email) && PasswordsMatch(_userRepository.GetHashedPassword(Email), HashedPassword))
                {
                    UserAccount account = _userRepository.GetUserByEmail(Email);
                    _userSessionManager.Create(account);
                    return Redirect("~/Restaurant/List");
                }
                else
                {
                    ModelState.AddModelError("InvalidCredentials", "Invalid login attempt.");
                }
            }

            return View("Login");

        }

        public IActionResult LogoutUser()
        {
            _userSessionManager.Destroy();
            return Redirect("../Home/Index");
        }

        public JsonResult EmailIsRegistered(string Email)
        {
            HttpContext.Session.SetString("Email", Email);
            if (!_userRepository.IsEmailRegistered(Email))
            {
                return Json(true);
            }

            return Json(false);
        }

        public Boolean EmailExists(string Email)
        {
            if (_userRepository.IsEmailRegistered(Email))
            {
                return true;
            }

            return false;
        }

        private Boolean PasswordsMatch(string savedPass, string inputPass)
        {
            return (savedPass.Equals(inputPass));
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
    }
}