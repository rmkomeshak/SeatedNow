using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SeatedNow.Models;
using SeatedNow.Models.SiteAdmin;

namespace SeatedNow.Repositories
{
    public class UserRepository : IUserRepository
    {
        SqlConnection connection;

        public UserRepository()
        {
            IDataRepository dataRepo = new DataRepository();
            connection = dataRepo.GetDBConnection();
        }

        public void RegisterNewUser(UserAccount account)
        {

            using (connection)
            {
                connection.Open();
                string sendquery = "INSERT INTO [dbo].[Users] VALUES ('" + account.getFirstName() + "', '" + account.getLastName() + "', '" + account.Email + "', '" + account.Password + "', '" + account.PhoneNumber + "', '" + account.Role + "')";

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (connection)
            {
                connection.Open();
                string sendquery = "DELETE FROM [dbo].[Users] WHERE id = '" + id + "'";    

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public bool UpdateUserAccount(UserAccount account)
        {
            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Users] SET firstname = '" + account.getFirstName()
                    + "', lastname = '" + account.getLastName() + "', email = '" + account.Email
                    + "', phone = '" + account.PhoneNumber + "', password = '" + account.Password
                    + "', role = '" + account.Role + "' WHERE id = " + account.UserID;

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
        }

        public string GetHashedPassword(string email)
        {

            string checkquery = "SELECT password FROM[dbo].[Users] WHERE email = '" + email + "'";
            string hashedPass;

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            hashedPass = (String) command.ExecuteScalar();

            connection.Close();

            return hashedPass;
        }

        public List<SiteAdminAccountListViewModel> GetUserListSiteAdmin()
        {
            List<SiteAdminAccountListViewModel> accounts = new List<SiteAdminAccountListViewModel>();

            string dbfirstname, dblastname, dbrole;
            int dbid;
            string checkquery = "SELECT id, firstname, lastname, role FROM [dbo].[Users]";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();
            StringBuilder fullname = new StringBuilder();

            while (reader.Read())
            {
                dbid = int.Parse(reader["id"].ToString());
                dbfirstname = reader["firstname"] as String ?? "";
                dblastname = reader["lastname"] as String ?? "";
                dbrole = reader["role"] as String ?? "";
                fullname.Clear();
                fullname.Append(dbfirstname).Append(" ").Append(dblastname);

                accounts.Add(new SiteAdminAccountListViewModel(dbid, fullname.ToString(), dbrole));
            }

            connection.Close();

            return accounts;
        }

        public UserAccount GetUserByEmail(string email)
        {
            int dbuserid = -1;
            string dbname = "", dbphone="", dbemail="", dbpass="", dbrole="";
            string checkquery = "SELECT id, firstname, lastname, phone, email, password, role FROM [dbo].[Users] WHERE email = '" + email + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbuserid = (int) reader["id"];
                dbname = reader["firstname"] + " " + reader["lastname"];
                dbphone = reader["phone"].ToString();
                dbemail = reader["email"].ToString();
                dbpass = reader["password"].ToString();
                dbrole = reader["role"].ToString();
            }

            connection.Close();

            return new UserAccount(dbuserid, dbname, dbemail, dbphone, dbpass, dbrole);
        }

        public UserAccount GetUserByFirstLastName(string firstname, string lastname)
        {
            throw new NotImplementedException();
        }

        public UserAccount GetUserByID(int id)
        {
            int dbuserid = -1;
            string dbname = "", dbphone = "", dbemail = "", dbpass = "", dbrole = "";
            string checkquery = "SELECT id, firstname, lastname, phone, email, password, role FROM [dbo].[Users] WHERE id = '" + id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbuserid = (int)reader["id"];
                dbname = reader["firstname"] + " " + reader["lastname"];
                dbphone = reader["phone"].ToString();
                dbemail = reader["email"].ToString();
                dbpass = reader["password"].ToString();
                dbrole = reader["role"].ToString();
            }

            connection.Close();

            return new UserAccount(dbuserid, dbname, dbemail, dbphone, dbpass, dbrole);
        }

        public List<DiningReservation> GetReservationsByCustomerID(int id)
        {
            List<DiningReservation> reservations = new List<DiningReservation>();

            int dbreservationid = -1, dbrestaurantid = -1, dbaccountid = -1, dbseatsreserved = -1, dbtableid = -1;
            string dbsection = "";
            DateTime dbreservationdatetime;
            string checkquery = "SELECT reservation_id, restaurant_id, account_id, seats_reserved, reservation_datetime, table_id, section FROM [dbo].[Reservations] WHERE account_id = '" + id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbreservationid = (int)reader["reservation_id"];
                dbrestaurantid = (int)reader["restaurant_id"];
                dbaccountid = (int)reader["account_id"];
                dbseatsreserved = (int)reader["seats_reserved"];
                dbreservationdatetime = (DateTime)reader["reservation_datetime"];
                dbtableid = (int)reader["table_id"];
                dbsection = (string)reader["section"];

                reservations.Add(new DiningReservation(dbreservationid, dbrestaurantid, dbaccountid, dbseatsreserved, dbreservationdatetime, dbtableid, dbsection));
            }

            connection.Close();

            return reservations;
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
