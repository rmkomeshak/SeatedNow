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

        private readonly IConfiguration configuration;
        private readonly SqlConnection connection;
        //Create the readonly configuration items for the database connection string

        public UsersRepository(IConfiguration config)
        {
            configuration = config;
            connection = new SqlConnection(configuration.GetConnectionString("SeatedNowDB"));
        }
        //Initialize the connection string and create connection object

        public void CreateUser(UserAccount account)
        {
            connection.Open();
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
