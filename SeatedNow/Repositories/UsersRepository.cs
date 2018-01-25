﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SeatedNow.Models;

namespace SeatedNow.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        SqlConnection connection;

        public UsersRepository()
        {
            DataRepository dataRepo = new DataRepository();
            connection = dataRepo.getDBConnection();
        }

        public void RegisterNewUser(UserAccount account)
        {

            try
            {
                using (connection)
                {
                    connection.Open();
                    string sendquery = "INSERT INTO [dbo].[Users] VALUES ('" + account.getFirstName() + "', '" + account.getLastName() + "', '" + account.Email + "', '" + account.Password + "', '" + account.PhoneNumber + "')";

                    using (SqlCommand command = new SqlCommand(sendquery, connection))
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }       
        }

        public void DeleteUser(UserAccount account)
        {
            throw new NotImplementedException();
        }

        public UserAccount GetUserByFirstLastName(string firstname, string lastname)
        {
            throw new NotImplementedException();
        }

        public UserAccount GetUserByID(int id)
        {
            throw new NotImplementedException();
        }

        public Boolean IsEmailRegistered(string email)
        {
            string checkquery = "SELECT count(*) FROM [dbo].[Users] WHERE email = '" + email + "'";
            int rows;

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);
        
            rows = (int) command.ExecuteScalar();

            connection.Close();

            if (rows > 0)
            {
                return true;
            } else
            {
                return false;
            }


        }
    }
}
