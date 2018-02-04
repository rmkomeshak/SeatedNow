using Microsoft.AspNetCore.Http;
using SeatedNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Managers
{
    public class UserSession
    {
        IHttpContextAccessor accessor = new HttpContextAccessor();

        public bool IsValid()
        {
            if (!String.IsNullOrEmpty(GetEmail()))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void Create(UserAccount account)
        {
            setID(account.UserID);
            SetFirstName(account.getFirstName());
            SetLastName(account.getLastName());
            SetEmail(account.Email);
            SetPhone(account.PhoneNumber);
        }

        public void Destroy()
        {
            accessor.HttpContext.Session.Remove("_id");
            accessor.HttpContext.Session.Remove("_firstname");
            accessor.HttpContext.Session.Remove("_lastname");
            accessor.HttpContext.Session.Remove("_email");
            accessor.HttpContext.Session.Remove("_phone");
        }

        public void setID(int id)
        {
            accessor.HttpContext.Session.SetString("_id", id.ToString());
        }

        public int getID()
        {
            return (int.Parse(accessor.HttpContext.Session.GetString("_id")));
        }

        public void SetFirstName(String firstname)
        {
            accessor.HttpContext.Session.SetString("_firstname", firstname);
        }

        public string GetFirstName()
        {
            return (accessor.HttpContext.Session.GetString("_firstname"));
        }

        public void SetLastName(String lastname)
        {
            accessor.HttpContext.Session.SetString("_lastname", lastname);
        }

        public string GetLastName()
        {
            return (accessor.HttpContext.Session.GetString("_lastname"));
        }

        public void SetEmail(String email)
        {
            accessor.HttpContext.Session.SetString("_email", email);
        }

        public string GetEmail()
        {
            return (accessor.HttpContext.Session.GetString("_email"));
        }

        public void SetPhone(String phone)
        {
            accessor.HttpContext.Session.SetString("_phone", phone);
        }

        public string GetPhone()
        {
            return (accessor.HttpContext.Session.GetString("_phone"));
        }
    }
}
