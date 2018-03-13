﻿using System;
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
        IStatsRepository _statsRepository = new StatsRepository();

        public RestaurantRepository()
        {
            IDataRepository dataRepo = new DataRepository();
            IStatsRepository statsRepo = new StatsRepository();
            connection = dataRepo.GetDBConnection();
        }

        public void RegisterNewRestaurant(Restaurant restaurant)
        {
            using (connection)
            {
                connection.Open();
                string sendqueryRestaurant = "INSERT INTO [dbo].[Restaurants] VALUES ('" + restaurant.Name + "', '"
                                    + restaurant.Address + "', '" + restaurant.City + "', '"
                                    + restaurant.State + "', '" + restaurant.ZipCode + "', '"
                                    + restaurant.PhoneNumber + "', '" + restaurant.ImagePath + "', '"
                                    + restaurant.IsVerified + "', '" + restaurant.OwnerId + "')";
           
                using (SqlCommand command = new SqlCommand(sendqueryRestaurant, connection))
                {
                    command.ExecuteNonQuery();
                }

                /*
                string sendqueryRestaurants_Stats = "INSERT INTO [dbo].[Restaurant_Stats] VALUES ('" + restaurant.Id + "', '0', '0', '0', '0')";

                using (SqlCommand command = new SqlCommand(sendqueryRestaurants_Stats, connection))
                {
                    command.ExecuteNonQuery();
                }
                */

                connection.Close();
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
                    + "', zipcode = '" + restaurant.ZipCode + "', state = '" + restaurant.State
                    + "', phone = '" + restaurant.PhoneNumber + "', image = '" + restaurant.ImagePath
                    + "', verified = '" + restaurant.IsVerified + "', owner_id = '" + restaurant.OwnerId
                    + "', event_key = '" + restaurant.EventKey + "', description = '" + restaurant.Description  
                    + "' WHERE id = " + restaurant.Id;

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
        }

        public bool UpdateRestaurantTable(RestaurantTableList t)
        {
            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Restaurant_Tables] SET restaurant_id = '" + t.RestaurantId
                    + "', table_name = '" + t.TableName + "', taken = '" + t.IsTaken
                    + "', reservation_id = '" + t.ReservationId + "', table_id = '" + t.TableId
                    + "' WHERE restaurant_id = " + t.RestaurantId + " AND table_id = " + t.TableId;

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
            int dbid = -1;
            string checkquery = "SELECT id, name, address, city, state, zipcode, image FROM [dbo].[Restaurants] WHERE verified = 'true'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbid = (int)reader["id"];
                dbname = reader["name"].ToString();
                dbaddress = reader["address"].ToString();
                dbcity = reader["city"].ToString();
                dbstate = reader["state"].ToString();
                dbzipcode = reader["zipcode"].ToString();
                dbimage = reader["image"].ToString();

                RestaurantStats restaurantStats = _statsRepository.GetStatsByRestaurantId(dbid);

                restaurants.Add(new RestaurantListViewModel(dbid, dbname, dbaddress, dbcity, dbstate, dbzipcode, dbimage, restaurantStats));
            }

            connection.Close();
            return restaurants;
        }

        public List<RestaurantListViewModel> GetRestaurantsByReservations()
        {
            List<RestaurantListViewModel> restaurants = new List<RestaurantListViewModel>();

            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "";
            int dbid = -1;
            string checkquery = "SELECT id, name, address, city, state, zipcode, image FROM [dbo].[Restaurants] AS m JOIN [dbo].[Restaurant_Stats] AS p on p.restaurant_id = m.id WHERE m.verified = 'true' ORDER BY p.reservations DESC";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbid = (int)reader["id"];
                dbname = reader["name"].ToString();
                dbaddress = reader["address"].ToString();
                dbcity = reader["city"].ToString();
                dbstate = reader["state"].ToString();
                dbzipcode = reader["zipcode"].ToString();
                dbimage = reader["image"].ToString();

                RestaurantStats restaurantStats = _statsRepository.GetStatsByRestaurantId(dbid);
                List<string> tags = _statsRepository.GetTagsByRestaurantID(dbid);

                restaurants.Add(new RestaurantListViewModel(dbid, dbname, dbaddress, dbcity, dbstate, dbzipcode, dbimage, restaurantStats, tags));
            }

            connection.Close();
            return restaurants;
        }

        public List<RestaurantListViewModel> GetRestaurantsByRatings()
        {
            List<RestaurantListViewModel> restaurants = new List<RestaurantListViewModel>();

            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "";
            int dbid = -1;
            string checkquery = "SELECT id, name, address, city, state, zipcode, image FROM [dbo].[Restaurants] AS m JOIN [dbo].[Restaurant_Stats] AS p on p.restaurant_id = m.id WHERE m.verified = 'true' ORDER BY p.rating DESC";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbid = (int)reader["id"];
                dbname = reader["name"].ToString();
                dbaddress = reader["address"].ToString();
                dbcity = reader["city"].ToString();
                dbstate = reader["state"].ToString();
                dbzipcode = reader["zipcode"].ToString();
                dbimage = reader["image"].ToString();

                RestaurantStats restaurantStats = _statsRepository.GetStatsByRestaurantId(dbid);
                List<string> tags = _statsRepository.GetTagsByRestaurantID(dbid);

                restaurants.Add(new RestaurantListViewModel(dbid, dbname, dbaddress, dbcity, dbstate, dbzipcode, dbimage, restaurantStats, tags));
            }

            connection.Close();
            return restaurants;
        }

        public List<RestaurantListViewModel> GetRestaurantsByWaitTime()
        {
            List<RestaurantListViewModel> restaurants = new List<RestaurantListViewModel>();

            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "";
            int dbid = -1;
            string checkquery = "SELECT id, name, address, city, state, zipcode, image FROM [dbo].[Restaurants] AS m JOIN [dbo].[Restaurant_Stats] AS p on p.restaurant_id = m.id WHERE m.verified = 'true' ORDER BY p.wait_time ASC";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbid = (int)reader["id"];
                dbname = reader["name"].ToString();
                dbaddress = reader["address"].ToString();
                dbcity = reader["city"].ToString();
                dbstate = reader["state"].ToString();
                dbzipcode = reader["zipcode"].ToString();
                dbimage = reader["image"].ToString();

                RestaurantStats restaurantStats = _statsRepository.GetStatsByRestaurantId(dbid);
                List<string> tags = _statsRepository.GetTagsByRestaurantID(dbid);

                restaurants.Add(new RestaurantListViewModel(dbid, dbname, dbaddress, dbcity, dbstate, dbzipcode, dbimage, restaurantStats, tags));
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
            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "", dbphone = "", dbeventkey = "", dbdescription = "";
            bool dbverified = false;
            string checkquery = "SELECT id, name, address, city, zipcode, state, phone, image, verified, owner_id, event_key, description FROM [dbo].[Restaurants] WHERE id = '" + id + "'";

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
                dbeventkey = reader["event_key"].ToString();
                dbdescription = reader["description"].ToString();
            }

            connection.Close();

            return new Restaurant(dbrestaurantid, dbname, dbaddress, dbcity, dbzipcode, dbstate, dbphone, dbimage, dbverified, dbownerid, dbeventkey, dbdescription);
        }

        public Restaurant GetRestaurantByOwnerID(int id)
        {
            int dbrestaurantid = -1, dbownerid = -1;
            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "", dbphone = "", dbeventkey = "", dbdescription = "";
            bool dbverified = false;
            string checkquery = "SELECT id, name, address, city, zipcode, state, phone, image, verified, owner_id, event_key, description FROM [dbo].[Restaurants] WHERE owner_id = '" + id + "'";

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
                dbeventkey = reader["event_key"].ToString();
                dbdescription = reader["description"].ToString();
            }

            connection.Close();

            return new Restaurant(dbrestaurantid, dbname, dbaddress, dbcity, dbzipcode, dbstate, dbphone, dbimage, dbverified, dbownerid, dbeventkey);
        }


        public List<RestaurantTableList> GetTablesByRestaurantID(int id)
        {
            List<RestaurantTableList> tablelist = new List<RestaurantTableList>();
            int dbrestaurantid = -1, dbreservationid = -1, dbtableid = -1;
            string dbtablename = "";
            bool dbtaken = false;

            string checkquery = "SELECT restaurant_id, table_name, taken, reservation_id, table_id FROM [dbo].[Restaurant_Tables] WHERE restaurant_id = '" + id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbrestaurantid = (int)reader["restaurant_id"];
                dbtablename = (string)reader["table_name"];
                dbtaken = (bool)reader["taken"];
                dbreservationid = (int)reader["reservation_id"];
                dbtableid = (int)reader["table_id"];
                tablelist.Add(new RestaurantTableList(dbrestaurantid, dbtablename, dbtaken, dbreservationid, dbtableid));
            }

            connection.Close();

            return tablelist;
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
