using System;
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

        public void CreateUser(UserAccount account)
        {

            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = "INSERT INTO [dbo].[Users]  VALUES ('" + account.FirstName + "', '" + account.LastName + "', '" + account.Email + "', '" + account.Password + "', '" + account.PhoneNumber + "')";

                    using (SqlCommand command = new SqlCommand(query, connection))
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
    }
}
