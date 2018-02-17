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
            try
            {
                using (connection)
                {
                    connection.Open();
                    string sendquery = "INSERT INTO [dbo].[Restaurants] VALUES ('" + restaurant.Name + "', '"
                                        + restaurant.Address + "', '" + restaurant.City + "', '"
                                        + restaurant.State + "', '" + restaurant.ZipCode + "', '"
                                        + restaurant.PhoneNumber + "', '" + restaurant.ImagePath + "')";

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

        public void DeleteRestaurant(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public List<RestaurantListViewModel> GetRestaurants()
        {
            List<RestaurantListViewModel> restaurants = new List<RestaurantListViewModel>();

            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "";
            string checkquery = "SELECT name, address, city, state, zipcode, image FROM [dbo].[Restaurants]";

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

        public Restaurant GetRestaurantByAddress(string address, string city, string state, string zipcode)
        {
            throw new NotImplementedException();
        }

        public Restaurant GetRestaurantByID(int id)
        {
            throw new NotImplementedException();
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
