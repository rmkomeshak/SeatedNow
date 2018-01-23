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

        [HttpPost]
        public IActionResult CreateUser(String FirstName, String LastName, String Email, String PhoneNumber, String Password)
        {

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Server = tcp:seatednow.database.windows.net,1433; Initial Catalog = seatednow; Persist Security Info = False; User ID = seatednow; Password = Sipawd123; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";

            connection.Open();

            string query = "INSERT INTO [dbo].[Users]  VALUES ('dane', 'mazzaro', 'damazzaro@oakland.edu', 'hello1234', '2484960964')";

            SqlCommand cmd = new SqlCommand(query, connection);

            connection.Close();
            return Content(FirstName + " | " + LastName + " | " + Email + " | " + Password );
        }
    }
}