using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        IStatsRepository _statsRepository = new StatsRepository();
        UserSession _userSessionManager = new UserSession();
        BlobsRepository _blobsRepository = new BlobsRepository();


        public IActionResult Index()
        {
            if (_userSessionManager == null || _userSessionManager.IsValid())
                return Redirect("~/Restaurant/List");

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

            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Tags = _statsRepository.GetTagsByRestaurantID(Id);

            return PartialView(restaurant);
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

        public IActionResult DetailsRestaurant(int Id)
        {
            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Tags = _statsRepository.GetTagsByRestaurantID(Id);

            return PartialView(restaurant);
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

       
        public IActionResult SendCreateRestaurant(string Name, string Address, string City, string ZipCode, string State, string PhoneNumber, string ImagePath, string Description, string Color, int OwnerId, string EventKey, bool isVerified, string Keyword1, string Keyword2, string Keyword3, string Website, int Price, IFormFile UploadedMenu, IFormFile UploadedLogo)
        {

            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }

            string regName = Regex.Replace(Name, "'", "''");
            string regAddress = Regex.Replace(Address, "'", "''");
            string regCity = Regex.Replace(City, "'", "''");
            string regDescription = Regex.Replace(Description, "'", "''");
            string regKeyword1 = Regex.Replace(Keyword1, "'", "''");
            string regKeyword2 = Regex.Replace(Keyword2, "'", "''");
            string regKeyword3 = Regex.Replace(Keyword3, "'", "''");

            Restaurant r = new Restaurant(regName, regAddress, regCity, ZipCode, State, PhoneNumber, ImagePath, isVerified, OwnerId, EventKey, regDescription, Color, regKeyword1, regKeyword2, regKeyword3, Website, Price);
            _restaurantRepository.RegisterNewRestaurant(r);


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

        public async Task<IActionResult> SendUpdateRestaurant(int Id, string Name, string Address, string City, string ZipCode, string State, string PhoneNumber, string ImagePath, string Description, string Color, int OwnerId, string EventKey, bool isVerified, string Keyword1, string Keyword2, string Keyword3, string Website, int Price, IFormFile UploadedMenu, IFormFile UploadedLogo, IFormFile UploadedFloorplan)
        {

            if (_userSessionManager.GetRole() != "Admin")
            {
                return Redirect("~/");
            }

            if (UploadedLogo != null)
            {
                var fileExtensionLogo = "." + UploadedLogo.ContentType.Substring(UploadedLogo.ContentType.LastIndexOf("/") + 1);
                string fileNameLogo = Id + Name + "Logo" + ".png";

                if (await _blobsRepository.DoesBlobExistAsync(fileNameLogo))
                {
                    await _blobsRepository.DeleteBlobAsync(fileNameLogo);
                    await _blobsRepository.UploadBlobAsync(UploadedLogo, fileNameLogo);
                }
                else
                {
                    await _blobsRepository.UploadBlobAsync(UploadedLogo, fileNameLogo);
                }
            }

            if (UploadedMenu != null)
            {
                var fileExtensionMenu = "." + UploadedMenu.ContentType.Substring(UploadedMenu.ContentType.LastIndexOf("/") + 1);
                string fileNameMenu = Id + Name + "Menu" + fileExtensionMenu;

                if (await _blobsRepository.DoesBlobExistAsync(fileNameMenu))
                {
                    await _blobsRepository.DeleteBlobAsync(fileNameMenu);
                    await _blobsRepository.UploadBlobAsync(UploadedMenu, fileNameMenu);
                }
                else
                {
                    await _blobsRepository.UploadBlobAsync(UploadedMenu, fileNameMenu);
                }
            }

            if (UploadedFloorplan != null)
            {
                var fileExtensionFloorplan = "." + UploadedFloorplan.ContentType.Substring(UploadedFloorplan.ContentType.LastIndexOf("/") + 1);
                string fileNameFloorplan = Id + Name + "Floorplan" + ".png";

                if (await _blobsRepository.DoesBlobExistAsync(fileNameFloorplan))
                {
                    await _blobsRepository.DeleteBlobAsync(fileNameFloorplan);
                    await _blobsRepository.UploadBlobAsync(UploadedFloorplan, fileNameFloorplan);
                }
                else
                {
                    await _blobsRepository.UploadBlobAsync(UploadedFloorplan, fileNameFloorplan);
                }
            }

            string regName = Regex.Replace(Name, "'", "''");
            string regAddress = Regex.Replace(Address, "'", "''");
            string regCity = Regex.Replace(City, "'", "''");
            string regDescription = Regex.Replace(Description, "'", "''");
            string regKeyword1 = Regex.Replace(Keyword1, "'", "''");
            string regKeyword2 = Regex.Replace(Keyword2, "'", "''");
            string regKeyword3 = Regex.Replace(Keyword3, "'", "''");

            Restaurant r = new Restaurant(Id, regName, regAddress, regCity, ZipCode, State, PhoneNumber, ImagePath, isVerified, OwnerId, EventKey, regDescription, Color, regKeyword1, regKeyword2, regKeyword3, Website, Price);
            _restaurantRepository.UpdateRestaurant(r);
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