using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        UserSession _userSessionManager = new UserSession();

        public IActionResult Login()
        {
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
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
            restaurant.Tables = _restaurantRepository.GetTablesByRestaurantID(Id);

            return PartialView(restaurant);
        }

        public IActionResult DashStatistics(int Id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantByID(Id);
            restaurant.Stats = _statsRepository.GetStatsByRestaurantId(Id);
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
            return View(restaurant);

        }

        public IActionResult ReserveAction(int guests, string section, int restaurant_id)
        {
            DateTime dt = DateTime.Today;
            DiningReservation r = new DiningReservation(restaurant_id, _userSessionManager.getID(), guests, dt, 10, section);
            _reservationRepository.CreateReservation(r);
            return Redirect("~/Restaurant/List");
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
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByReservations(), _restaurantRepository.GetRestaurantsByReservations());
                    ViewBag.SortBy = "Most Reserved Restaurants";
                    break;
                case "ratings":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByRatings(), _restaurantRepository.GetRestaurantsByRatings());
                    ViewBag.SortBy = "Highest Rated Restaurants";
                    break;
                case "waittime":
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByWaitTime(), _restaurantRepository.GetRestaurantsByWaitTime());
                    ViewBag.SortBy = "Shortest Wait Time";
                    break;
                default:
                    contents = new ListPage(_reservationRepository.GetReservationsByCustomerID(userId, 5), _restaurantRepository.GetRestaurantsByReservations(), _restaurantRepository.GetRestaurantsByReservations());
                    ViewBag.SortBy = "Most Reserved Restaurants";
                    break;
            }

            return View(contents);
        }

        public IActionResult SendRating(float rating, string comment, int restaurant_id)
        {

            int userId = _userSessionManager.getID();
            DateTime time = DateTime.Now;

            _restaurantRepository.InputRating(restaurant_id, rating, comment, time, userId);

            return Redirect(Request.Headers["Referer"].ToString());

        }

        public IActionResult UpdateHours(int MondayOpen, int MondayClose, int TuesdayOpen, int TuesdayClose, int WednsedayOpen, int WednsedayClose, int ThursdayOpen, int ThursdayClose, int FridayOpen, int FridayClose, int SaturdayOpen, int SaturdayClose, int SundayOpen, int SundayClose, int RestaurantId)
        {
            RestaurantHours Hours = new RestaurantHours(MondayOpen, MondayClose, TuesdayOpen, TuesdayClose, WednsedayOpen, WednsedayClose, ThursdayOpen, ThursdayClose, FridayOpen, FridayClose, SaturdayOpen, SaturdayClose, SundayOpen, SundayClose);
            _statsRepository.SetHoursByRestaurantId(RestaurantId, Hours);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult SearchRestaurants(string searchquery)
        {
            SearchContent content;

            if (searchquery[0].Equals('#'))
            {
                List<string> tags = new List<string>();
                tags.Add(searchquery.Substring(1));
                content = new SearchContent(searchquery, _restaurantRepository.GetRestaurantsByTags(tags));

            }
            else
            {
                content = new SearchContent(searchquery, _restaurantRepository.GetRestaurantsByTags(_statsRepository.GetTagsByRestaurantName(searchquery)));
            }

            return View(content);
        }

        public IActionResult UpdateAction(int Id, string Name, string Address, string City, string ZipCode, string State, string PhoneNumber, string ImagePath, string Description, string Color, int OwnerId, string EventKey, bool isVerified, string Keyword1, string Keyword2, string Keyword3)
        {
            Restaurant r = new Restaurant(Id, Name, Address, City, ZipCode, State, PhoneNumber, ImagePath, isVerified, OwnerId, EventKey, Description, Color, Keyword1, Keyword2, Keyword3);

            _restaurantRepository.UpdateRestaurant(r);
            _restaurantRepository.UpdateTags(r.Tags, Id);
            return Redirect("~/Restaurant/Dashboard");
        }


    }
}