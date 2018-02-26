using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SeatedNow.Models;

namespace SeatedNow.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {

        SqlConnection connection;

        public RestaurantRepository()
        {
            IDataRepository dataRepo = new DataRepository();
            connection = dataRepo.GetDBConnection();
        }

        public void RegisterNewRestaurant(Restaurant restaurant)
        {
            using (connection)
            {
                connection.Open();
                string sendquery = "INSERT INTO [dbo].[Restaurants] VALUES ('" + restaurant.Name + "', '"
                                    + restaurant.Address + "', '" + restaurant.City + "', '"
                                    + restaurant.State + "', '" + restaurant.ZipCode + "', '"
                                    + restaurant.PhoneNumber + "', '" + restaurant.ImagePath + "', '"
                                    + restaurant.IsVerified + "', '" + restaurant.OwnerId + "')";

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void DeleteRestaurant(int id)
        {
            using (connection)
            {
                connection.Open();
                string sendquery = "DELETE FROM [dbo].[Restaurants] WHERE id = '" + id + "'";

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public bool UpdateRestaurant(Restaurant restaurant)
        {
            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Restaurants] SET name = '" + restaurant.Name
                    + "', address = '" + restaurant.Address + "', city = '" + restaurant.City
                    + "', state = '" + restaurant.State + "', zipcode = '" + restaurant.ZipCode
                    + "', phone = '" + restaurant.PhoneNumber + "', image = '" + restaurant.ImagePath
                    + "', verified = '" + restaurant.IsVerified + "', owner_id = '" + restaurant.OwnerId
                    + "' WHERE id = " + restaurant.Id;

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
        }

        public List<RestaurantListViewModel> GetRestaurants()
        {
            List<RestaurantListViewModel> restaurants = new List<RestaurantListViewModel>();

            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "";
            string checkquery = "SELECT name, address, city, state, zipcode, image FROM [dbo].[Restaurants] WHERE verified = 'true'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbname = reader["name"].ToString();
                dbaddress = reader["address"].ToString();
                dbcity = reader["city"].ToString();
                dbstate = reader["state"].ToString();
                dbzipcode = reader["zipcode"].ToString();
                dbimage = reader["image"].ToString();

                restaurants.Add(new RestaurantListViewModel(dbname, dbaddress, dbcity, dbstate, dbzipcode, dbimage));
            }

            connection.Close();
            return restaurants;
        }

        public List<SideAdminRestaurantListViewModel> GetRestaurantsAdminList()
        {
            List<SideAdminRestaurantListViewModel> restaurants = new List<SideAdminRestaurantListViewModel>();

            string dbname;
            int dbid;
            bool dbisVerified;
            string checkquery = "SELECT id, name, verified FROM [dbo].[Restaurants]";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbid = int.Parse(reader["id"].ToString());
                dbname = reader["name"] as String ?? "";
                dbisVerified = reader.GetBoolean(reader.GetOrdinal("verified"));

                restaurants.Add(new SideAdminRestaurantListViewModel(dbid, dbname, dbisVerified));
            }

            connection.Close();
            return restaurants;
        }

        public Restaurant GetRestaurantByAddress(string address, string city, string state, string zipcode)
        {
            throw new NotImplementedException();
        }

        public Restaurant GetRestaurantByID(int id)
        {
            int dbrestaurantid = -1, dbownerid = -1;
            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "", dbphone = "";
            bool dbverified = false;
            string checkquery = "SELECT id, name, address, city, state, zipcode, phone, image, verified, owner_id FROM [dbo].[Restaurants] WHERE id = '" + id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbrestaurantid = (int)reader["id"];
                dbname = reader["name"].ToString();
                dbaddress = reader["address"].ToString();
                dbcity = reader["city"].ToString();
                dbstate = reader["state"].ToString();
                dbzipcode = reader["zipcode"].ToString();
                dbimage = reader["image"].ToString();
                dbphone = reader["phone"].ToString();
                dbverified = reader.GetBoolean(reader.GetOrdinal("verified"));
                dbownerid = (int)reader["owner_id"];
            }

            connection.Close();

            return new Restaurant(dbrestaurantid, dbname, dbaddress, dbcity, dbstate, dbzipcode, dbphone, dbimage, dbverified, dbownerid);
        }

        public Restaurant GetrestaurantByPhone(string phone)
        {
            throw new NotImplementedException();
        }

        public bool IsRestaurantRegistered(string email)
        {
            throw new NotImplementedException();
        }
    }
}
