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

        public UsersRepository()
        {
            DataRepository dataRepo = new DataRepository();
        }

        public void CreateUser(UserAccount account)
        {
            /*
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Server = tcp:seatednow.database.windows.net,1433; Initial Catalog = seatednow; Persist Security Info = False; User ID = seatednow; Password = Sipawd123; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";

            connection.Open();

            string query = "INSERT INTO [dbo].[Users]  VALUES ('dane', 'mazzaro', 'damazzaro@oakland.edu', 'hello1234', '2484960964')";

            SqlCommand cmd = new SqlCommand(query, connection);

            connection.Close();
            */
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
