using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Managers;
using SeatedNow.Models;
using SeatedNow.Repositories;

namespace SeatedNow.Controllers
{
    public class RestaurantController : Controller
    {
        IRestaurantRepository _restaurantRepository = new RestaurantRepository();
        IStatsRepository _statsRepository = new StatsRepository();
        IReservationRepository _reservationRepository = new ReservationRepository();
        IUserRepository _userRepository = new UserRepository();
        BlobsRepository _blobsRepository = new BlobsRepository();
        UserSession _userSessionManager = new UserSession();

        public IActionResult Login()
        {
            if (_userSessionManager == null || _userSessionManager.IsValid())
                return Redirect("~/Restaurant/List");

            return View();
        }

        public IActionResult Designer()
        {
            if (_userSessionManager == null || !_userSessionManager.IsValid())
                return Redirect("~/");
            else if (_userSessionManager.GetRole().Equals("General") && _userSessionManager.IsValid())
                return Redirect("~/Restaurant/List");

            return View();
        }

        public IActionResult FloorPlan()
        {
            if (_userSessionManager == null || !_userSessionManager.IsValid())
                return Redirect("~/");
            else if (_userSessionManager.GetRole().Equals("General") && _userSessionManager.IsValid())
                return Redirect("~/Restaurant/List");

            return View(_restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID()));
        }

        public IActionResult Dashboard()
        {
            if (_userSessionManager == null || !_userSessionManager.IsValid())
                return Redirect("~/");
            else if (_userSessionManager.GetRole().Equals("General") && _userSessionManager.IsValid())
                return Redirect("~/Restaurant/List");

            Restaurant restaurant = _restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID());
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(restaurant.Id);
            restaurant.Tables = _restaurantRepository.GetTablesByRestaurantID(restaurant.Id);
            return View(restaurant);

        }

        public IActionResult DashOverview(int Id)
        {
            string name = "";
            string table_name = "";
            UserAccount u;
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Tables = _restaurantRepository.GetTablesByRestaurantID(Id);
            restaurant.Sections = _restaurantRepository.GetSections(Id);
            List<DiningReservation> reservations = _reservationRepository.GetReservationsByRestaurantId(Id);
            List<DiningReservation> resviews = new List<DiningReservation>();

            foreach(var reservation in reservations)
            {
                if(reservation.OwnerID != restaurant.OwnerId)
                {
                    u = _userRepository.GetUserByID(reservation.OwnerID);
                    name = u.getFirstName() + " " + u.getLastName();
                    table_name = reservation.Section + " " + reservation.TableID;
                    resviews.Add(new DiningReservation(name, table_name, reservation.Guests));
                }
            }
            DashOverview content = new DashOverview(restaurant, resviews);

            return PartialView(content);
        }

        public IActionResult DashStatistics(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Tables = _restaurantRepository.GetTablesByRestaurantID(Id);
            restaurant.Ratings = _statsRepository.GetRatingsByRestaurantId(Id);
            List<RestaurantListViewModel> other = _restaurantRepository.GetRestaurants();
            DashStats content = new DashStats(restaurant, other);

            return PartialView(content);
        }

        public IActionResult DashProfile(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashSettingsContact(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Tags = _statsRepository.GetTagsByRestaurantID(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashSettingsProfile(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Tags = _statsRepository.GetTagsByRestaurantID(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashSettingsHours(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Hours = _statsRepository.GetHoursByRestaurantId(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashFloorplan(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Sections = _restaurantRepository.GetSections(Id);

            return PartialView(restaurant);
        }

        public IActionResult Listing(int Id)
        {
            if (_userSessionManager == null || !_userSessionManager.IsValid())
                return Redirect("~/");

            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Tags = _statsRepository.GetTagsByRestaurantID(Id);
            restaurant.Ratings = _statsRepository.GetRatingsByRestaurantId(Id);
            restaurant.Hours = _statsRepository.GetHoursByRestaurantId(Id);
            restaurant.Sections = _restaurantRepository.GetSections(_restaurantRepository.GetRestaurantByID(Id).Id);
            return View(restaurant);

        }

        public IActionResult ReserveAction(int guests, string section, int restaurant_id)
        {
            DateTime dt = DateTime.Today;
            List<RestaurantTableList> tables = _restaurantRepository.GetTablesByRestaurantID(restaurant_id);
            List<RestaurantSection> sections = _restaurantRepository.GetSections(restaurant_id);

            int sec = -1;

            foreach(var s in sections)
            {
                if(s.Name.ToLower().Equals(section.ToLower()))
                {
                    sec = s.Id;
                    break;
                }
            }

            foreach(var table in tables)
            {
                if ((!table.IsTaken) && (table.Section == sec))
                {
                    int i = table.TableName.LastIndexOf(" ");
                    string sectionName = "Test";
                    int table_id = 1;

                    if (i != -1)
                    {
                        sectionName = table.TableName.Substring(0, i).ToLower();
                        table_id = Int32.Parse(table.TableName.Substring(i + 1));
                    }

                    DiningReservation r = new DiningReservation(restaurant_id, _userSessionManager.getID(), guests, DateTime.Now, table_id, section);
                    _reservationRepository.CreateReservation(r);
                    _statsRepository.UpdateCustomers(guests, restaurant_id);
                    _restaurantRepository.OccupyTable(restaurant_id, table.TableName, r.Time);

                    
                    break;
                }
            }
            return Redirect("~/Restaurant/List");
        }

        public IActionResult UpdatePageviews(int rId)
        {
            _statsRepository.UpdatePageviews(rId);
            string listing_page = "~/Restaurant/Listing/" + rId;
            return Redirect(listing_page);
        }

        public IActionResult List(string SortBy="", int resid=0)
        {
            if (_userSessionManager == null || !_userSessionManager.IsValid())
                return Redirect("~/");
           
            int userId = _userSessionManager.getID();
            ListPage contents;

            if(resid > 0)
            {
                _reservationRepository.DeleteReservation(resid);
            }

            switch (SortBy)
            {
                case "reservations":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByReservations());
                    ViewBag.SortBy = "Most Reserved Restaurants";
                    break;
                case "ratings":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByRatings());
                    ViewBag.SortBy = "Highest Rated Restaurants";
                    break;
                case "waittime":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByWaitTime());
                    ViewBag.SortBy = "Shortest Wait Time";
                    break;
                default:
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByReservations());
                    ViewBag.SortBy = "Most Reserved Restaurants";
                    break;
            }

            return View(contents);
        }
        
        public IActionResult ListAll(string SortBy = "", int resid = 0)
        {
            if (_userSessionManager == null || !_userSessionManager.IsValid())
                return Redirect("~/");

            int userId = _userSessionManager.getID();
            ListPage contents;

            if (resid > 0)
            {
                _reservationRepository.DeleteReservation(resid);
            }

            switch (SortBy)
            {
                case "reservations":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByReservations());
                    ViewBag.SortBy = "Sorted by Most Reserved";
                    break;
                case "ratings":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByRatings());
                    ViewBag.SortBy = "Sorted by Highest Rated";
                    break;
                case "waittime":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByWaitTime());
                    ViewBag.SortBy = "Sorted by Wait Time";
                    break;
                default:
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByReservations());
                    ViewBag.SortBy = "Sorted by Most Reserved";
                    break;
            }

            return View(contents);
        }

        public IActionResult SendRating(float rating, string comment, int restaurant_id)
        {

            int userId = _userSessionManager.getID();
            DateTime time = DateTime.Now;
            comment = Regex.Replace(comment, "'", "''");

            _restaurantRepository.InputRating(restaurant_id, rating, comment, time, userId);

            return Redirect(Request.Headers["Referer"].ToString());

        }

        public IActionResult UpdateHours(int MondayOpen, int MondayClose, int TuesdayOpen, int TuesdayClose, int WednsedayOpen, int WednsedayClose, int ThursdayOpen, int ThursdayClose, int FridayOpen, int FridayClose, int SaturdayOpen, int SaturdayClose, int SundayOpen, int SundayClose, int RestaurantId)
        {
            RestaurantHours Hours = new RestaurantHours(MondayOpen, MondayClose, TuesdayOpen, TuesdayClose, WednsedayOpen, WednsedayClose, ThursdayOpen, ThursdayClose, FridayOpen, FridayClose, SaturdayOpen, SaturdayClose, SundayOpen, SundayClose);
            _statsRepository.SetHoursByRestaurantId(RestaurantId, Hours);
            TempData["successMessage"] = "Success";
            return Redirect("~/Restaurant/Dashboard");
        }

        public IActionResult SearchRestaurants(string searchquery)
        {
            SearchContent content;
            var searchquery2 = "";
            searchquery2 += searchquery;

            if (searchquery.Contains("'"))
            {
                searchquery = Regex.Replace(searchquery, "'", "''");
            }
            if (searchquery.Contains("%"))
            {
                return Redirect("~/Restaurant/List");
            }
            if (searchquery.Contains(";"))
            {
                return Redirect("~/Restaurant/List");
            }
            if (searchquery.Contains("/"))
            {
                return Redirect("~/Restaurant/List");
            }
            if (searchquery.Contains("("))
            {
                return Redirect("~/Restaurant/List");
            }
            if (searchquery.Contains(")"))
            {
                return Redirect("~/Restaurant/List");
            }

            if (searchquery[0].Equals('#'))
                {
                    List<string> tags = new List<string>();
                    tags.Add(searchquery.Substring(1));
                    content = new SearchContent(searchquery2, _restaurantRepository.GetRestaurantsByTags(tags));

                }
                else
                {
                    content = new SearchContent(searchquery2, _restaurantRepository.GetRestaurantsByTags(_statsRepository.GetTagsByRestaurantName(searchquery)));
                }

            return View(content);
        }

        public async Task<IActionResult> UpdateAction(int Id, string Name, string Address, string City, string ZipCode, string State, string PhoneNumber, string ImagePath, string Description, string Color, int OwnerId, string EventKey, bool isVerified, string Keyword1, string Keyword2, string Keyword3, string Website, int Price, IFormFile UploadedMenu, IFormFile UploadedLogo, IFormFile UploadedFloorplan)
        {

            if (UploadedLogo != null)
            {
                var fileExtensionLogo = "." + UploadedLogo.ContentType.Substring(UploadedLogo.ContentType.LastIndexOf("/") + 1);
                string fileNameLogo = Id + Name + "Logo" + ".png";

                if (await _blobsRepository.DoesBlobExistAsync(fileNameLogo))
                {
                    await _blobsRepository.DeleteBlobAsync(fileNameLogo);
                    await _blobsRepository.UploadBlobAsync(UploadedLogo, fileNameLogo);
                } else
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

            if (String.IsNullOrEmpty(Description))
            {
                Description = "";
            }

            string regName = Regex.Replace(Name, "'", "''");
            Console.WriteLine("--------------------------------------" + regName);
            string regAddress = Regex.Replace(Address, "'", "''");
            Console.WriteLine("--------------------------------------" + regAddress);
            string regCity = Regex.Replace(City, "'", "''");
            Console.WriteLine("--------------------------------------" + regCity);
            string regDescription = Regex.Replace(Description, "'", "''");
            Console.WriteLine("--------------------------------------" + regDescription);
            string regKeyword1 = Regex.Replace(Keyword1, "'", "''");
            Console.WriteLine("--------------------------------------" + regKeyword1);
            string regKeyword2 = Regex.Replace(Keyword2, "'", "''");
            Console.WriteLine("--------------------------------------" + regKeyword2);
            string regKeyword3 = Regex.Replace(Keyword3, "'", "''");
            Console.WriteLine("--------------------------------------" + regKeyword3);

            Restaurant r = new Restaurant(Id, regName, regAddress, regCity, ZipCode, State, PhoneNumber, ImagePath, isVerified, OwnerId, EventKey, regDescription, Color, regKeyword1, regKeyword2, regKeyword3, Website, Price);
            TempData["successMessage"] = "Success";

            _restaurantRepository.UpdateRestaurant(r);
            return Redirect("~/Restaurant/Dashboard");
        }

        public IActionResult UpdateSections(string Section1Name = "", string Section2Name = "", string Section3Name = "", string Section4Name = "", string Section5Name = "", string Section6Name = "", int Section1Seats = -1, int Section2Seats = -1, int Section3Seats = -1, int Section4Seats = -1, int Section5Seats = -1, int Section6Seats = -1)
        {
            List<RestaurantTableList> tables = new List<RestaurantTableList>();
            Restaurant r = _restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID());
            int restaurant_id = r.Id;
            string tablename = "";
            int totalseats = 0;

            if ((!string.IsNullOrEmpty(Section1Name)) && (Section1Seats > 0))
            {
                for (int i = 1; i <= Section1Seats; i++)
                {
                    tablename = "";
                    tablename = Section1Name + " " + i;
                    totalseats++;
                    tables.Add(new RestaurantTableList(restaurant_id, tablename, false, -1, totalseats, 1, true));
                }  
            }

            if ((!string.IsNullOrEmpty(Section2Name)) && (Section2Seats > 0))
            {
                for (int i = 1; i <= Section2Seats; i++)
                {
                    tablename = "";
                    tablename = Section2Name + " " + i;
                    totalseats++;
                    tables.Add(new RestaurantTableList(restaurant_id, tablename, false, -1, totalseats, 2, true));
                }
            }

            if ((!string.IsNullOrEmpty(Section3Name)) && (Section3Seats > 0))
            {
                for (int i = 1; i <= Section3Seats; i++)
                {
                    tablename = "";
                    tablename = Section3Name + " " + i;
                    totalseats++;
                    tables.Add(new RestaurantTableList(restaurant_id, tablename, false, -1, totalseats, 3, true));
                }
            }

            if ((!string.IsNullOrEmpty(Section4Name)) && (Section4Seats > 0))
            {
                for (int i = 1; i <= Section4Seats; i++)
                {
                    tablename = "";
                    tablename = Section4Name + " " + i;
                    totalseats++;
                    tables.Add(new RestaurantTableList(restaurant_id, tablename, false, -1, totalseats, 4, true));
                }
            }

            if ((!string.IsNullOrEmpty(Section5Name)) && (Section5Seats > 0))
            {
                for (int i = 1; i <= Section5Seats; i++)
                {
                    tablename = "";
                    tablename = Section5Name + " " + i;
                    totalseats++;
                    tables.Add(new RestaurantTableList(restaurant_id, tablename, false, -1, totalseats, 5, true));
                }
            }

            if ((!string.IsNullOrEmpty(Section6Name)) && (Section6Seats > 0))
            {
                for (int i = 1; i <= Section6Seats; i++)
                {
                    tablename = "";
                    tablename = Section6Name + " " + i;
                    totalseats++;
                    tables.Add(new RestaurantTableList(restaurant_id, tablename, false, -1, totalseats, 6, true));
                }
            }

            _restaurantRepository.UpdateSections(restaurant_id, Section1Name, Section2Name, Section3Name, Section4Name, Section5Name, Section6Name, tables);

            TempData["successMessage"] = "Success";
            return Redirect("~/Restaurant/Dashboard");
        }

        public IActionResult Occupy(int table_id)
        {
            int restaurant_id = _restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID()).Id;
            _restaurantRepository.OccupyTable(restaurant_id, table_id);
            _statsRepository.RefreshWaitTime(restaurant_id);
            return Redirect("~/Restaurant/Dashboard");
        }

        public IActionResult Free(string table_name)
        {
            int i = table_name.LastIndexOf(" ");
            string section_name = "Test";
            int table_id = 1;

            if (i != -1)
            {
                section_name = table_name.Substring(0, i).ToLower();
                table_id = Int32.Parse(table_name.Substring(i + 1));
            }

            Restaurant r = _restaurantRepository.GetRestaurantByOwnerID(_userSessionManager.getID());
            int restaurant_id = r.Id;
            int guests = _restaurantRepository.GetGuests(restaurant_id, table_id, section_name);

            _restaurantRepository.FreeTable(restaurant_id, table_name);
            _statsRepository.ReduceCustomers(guests, restaurant_id);
            //_statsRepository.RefreshWaitTime(restaurant_id);
            //_reservationRepository.DeleteReservation(restaurant_id, table_id, section_name);
            _reservationRepository.DisableReservation(restaurant_id, table_id, section_name);


            return Redirect("~/Restaurant/Dashboard");
        }

        public IActionResult RefreshWaits()
        {
            _restaurantRepository.GetRestaurants();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult RestaurantReserve(string tableId, int guests, int restaurantId)
        {
            int i = tableId.LastIndexOf(" ");
            string sectionName = "Test";
            int table_id = 1;

            if(i != -1)
            {
                sectionName = tableId.Substring(0, i).ToLower();
                table_id = Int32.Parse(tableId.Substring(i + 1));
            }

            DiningReservation dr = new DiningReservation(restaurantId, _userSessionManager.getID(), guests, DateTime.Now, table_id, sectionName);
            _reservationRepository.CreateReservation(dr);
            _restaurantRepository.OccupyTable(restaurantId, tableId.ToLower(), dr.Time);
            _statsRepository.UpdateCustomers(guests, restaurantId);
            //_statsRepository.RefreshWaitTime(restaurantId);
         
            return Redirect("~/Restaurant/Dashboard");
        }
    }
}