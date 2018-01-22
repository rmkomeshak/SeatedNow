using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatedNow.Models;

namespace SeatedNow.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        SqlConnection con;

        public void CreateUser(UserAccount account)
        {
            builder.DataSource = "seatednow.database.microsoft.net";
            builder.UserID = "seatednow";
            builder.Password = "Sipawd123";
            builder.InitialCatalog = "SeatedNow";

            con = new SqlConnection(builder.ConnectionString);

            StringBuilder sb = new StringBuilder();
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
