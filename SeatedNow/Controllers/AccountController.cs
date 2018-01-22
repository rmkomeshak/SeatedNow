using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeatedNow.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection con = new SqlConnection();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        protected void  CreateUser()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();

        }
    }
}