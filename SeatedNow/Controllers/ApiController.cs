using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeatedNow.Repositories;
using SeatedNow.Models;

namespace SeatedNow.Controllers
{
    public class ApiController : Controller
    {

        IUserRepository _customerRepo = new UserRepository();
        AccountController accountController = new AccountController();
        public JsonResult LoginAuth()
        {

            if (Request.Query["email"].ToString() != null && Request.Query["pass"].ToString() != null){
                string email = Request.Query["email"].ToString();
                string pass = Request.Query["pass"].ToString();

                if (_customerRepo.IsEmailRegistered(email) && PasswordsMatch(_customerRepo.GetHashedPassword(email), pass)){

                    UserAccount customer = _customerRepo.GetUserByEmail(email);

                    return Json(customer);
                }
            }


            return Json("Nothing Found");
        }

    private Boolean PasswordsMatch(string savedPass, string inputPass)
    {
        return (savedPass.Equals(inputPass));
    }

    }
}