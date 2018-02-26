using SeatedNow.Models;
using SeatedNow.Models.SiteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    interface IUserRepository
    {
        void RegisterNewUser(UserAccount account);
        void DeleteUser(int id);
        List<SiteAdminAccountListViewModel> GetUserListSiteAdmin();
        UserAccount GetUserByID(int id);
        UserAccount GetUserByEmail(string email);
        bool UpdateUserAccount(UserAccount account);
        Boolean IsEmailRegistered(string email);
        string GetHashedPassword(string email);
    }
}
