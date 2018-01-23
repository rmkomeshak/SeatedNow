﻿using SeatedNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    interface IUsersRepository
    {
        void CreateUser(UserAccount account);

        void DeleteUser(UserAccount account);

        UserAccount GetUserByID(int id);

        UserAccount GetUserByFirstLastName(string firstname, string lastname);
    }
}