using SeatedNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    interface IUserRepository
    {
        void RegisterNewUser(UserAccount account);
        void DeleteUser(UserAccount account);

        UserAccount GetUserByID(int id);
        UserAccount GetUserByFirstLastName(string firstname, string lastname);
        UserAccount GetUserByEmail(string email);
        Boolean IsEmailRegistered(string email);
        string GetHashedPassword(string email);
    }
}
