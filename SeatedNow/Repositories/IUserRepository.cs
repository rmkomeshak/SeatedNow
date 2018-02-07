using SeatedNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    interface IUserRepository
    {
        void RegisterNewUser(CustomerAccount account);
        void DeleteUser(CustomerAccount account);

        CustomerAccount GetUserByID(int id);
        CustomerAccount GetUserByFirstLastName(string firstname, string lastname);
        CustomerAccount GetUserByEmail(string email);
        Boolean IsEmailRegistered(string email);
        string GetHashedPassword(string email);
    }
}
