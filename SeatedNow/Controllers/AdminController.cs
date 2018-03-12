using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Managers;
using SeatedNow.Models;
using SeatedNow.Repositories;

namespace SeatedNow.Controllers
{
    public class AdminController : Controller
    {
        IUserRepository _userRepository = new UserRepository();
        IRestaurantRepository _restaurantRepository = new RestaurantRepository();
        UserSession _userSessionManager = new UserSession();


        public IActionResult Index()
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return View();
        }

        public IActionResult Restaurants()
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return View(_restaurantRepository.GetRestaurantsAdminList());
        }

        public IActionResult Accounts()
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return View(_userRepository.GetUserListSiteAdmin());
        }

        public IActionResult UpdateAccount(int Id)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return PartialView(_userRepository.GetUserByID(Id));
        }

        public IActionResult UpdateRestaurant(int Id)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return PartialView(_restaurantRepository.GetRestaurantByID(Id));
        }

        public IActionResult CreateAccount(int Id)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return PartialView();
        }

        public IActionResult CreateRestaurant(int Id)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return PartialView();
        }

        public IActionResult DetailsAccount(int id)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return PartialView(_userRepository.GetUserByID(id));
        }

        public IActionResult DetailsRestaurant(int id)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            return PartialView(_restaurantRepository.GetRestaurantByID(id));
        }

        public IActionResult SendCreateAccount(string Name, string Email, string PhoneNumber, string Password, string Role)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }

            Password = _userRepository.GetHashedPassword(Password);
            UserAccount account = new UserAccount(Name, Email, PhoneNumber, Password, Role);
            _userRepository.RegisterNewUser(account);
            return Redirect("Accounts");
        }


        public IActionResult SendUpdateAccount(int UserId, string Name, string Email, string PhoneNumber, string Password, string Role)
        {

            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }

            UserAccount oldAccount = _userRepository.GetUserByID(UserId);

            if (String.IsNullOrEmpty(Name))
            {
                Name = oldAccount.Name;
            }

            if (String.IsNullOrEmpty(Password))
            {
                Password = _userRepository.GetHashedPassword(Email);
            }

            if (String.IsNullOrEmpty(Email))
            {
                Email = oldAccount.Email;
            }

            if (String.IsNullOrEmpty(PhoneNumber))
            {
                PhoneNumber = oldAccount.PhoneNumber;
            }

            if (String.IsNullOrEmpty(Role))
            {
                Role = oldAccount.Role;
            }


            Password = GenerateHash(Password);

            UserAccount account = new UserAccount(UserId, Name, Email, PhoneNumber, Password, Role);
            _userRepository.UpdateUserAccount(account);
            return Redirect("Accounts");
        }

        public IActionResult SendDeleteAccount(int id)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }

            _userRepository.DeleteUser(id);
            return Redirect("~/Admin/Accounts");
        }


        public IActionResult SendCreateRestaurant(string Name, string Address, string City, string ZipCode, string State, string PhoneNumber, string ImagePath, bool IsVerified, int OwnerId)
        {

            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }

            Restaurant restaurant = new Restaurant(Name, Address, City, ZipCode, State, PhoneNumber, ImagePath, IsVerified, OwnerId);
            _restaurantRepository.RegisterNewRestaurant(restaurant);

            if (_userSessionManager.GetRole() == "Admin")
            {
                return Redirect("Restaurants");
            }
            else if (_userSessionManager.GetRole() == "Restaurant")
            {
                return Redirect("~/Account");
            }
            else
            {
                return Redirect("~/Account");
            }
        }

        public IActionResult SendUpdateRestaurant(int Id, string Name, string Address, string City, string ZipCode, string State, string PhoneNumber, string ImagePath, bool IsVerified, int OwnerId)
        {

            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }

            Restaurant restaurant = new Restaurant(Id, Name, Address, City, ZipCode, State, PhoneNumber, ImagePath, IsVerified, OwnerId);
            _restaurantRepository.UpdateRestaurant(restaurant);
            return Redirect("Restaurants");
        }

        public IActionResult SendDeleteRestaurant(int id)
        {

            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }

            _restaurantRepository.DeleteRestaurant(id);
            return Redirect("~/Admin/Restaurants");
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