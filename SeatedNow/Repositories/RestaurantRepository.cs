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

        public List<RestaurantListViewModel> GetRestaurantsByName(string name)
        {
            List<RestaurantListViewModel> restaurants = new List<RestaurantListViewModel>();

            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "";
            int dbid = -1;
            string checkquery = "SELECT id, name, address, city, state, zipcode, image FROM [dbo].[Restaurants] WHERE name = '" + name + "'";

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

        public List<RestaurantListViewModel> SearchRestaurantsByName(string name)
        {
            List<RestaurantListViewModel> restaurants = new List<RestaurantListViewModel>();

            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "";
            int dbid = -1;
            string checkquery = "SELECT id, name, address, city, state, zipcode, image FROM [dbo].[Restaurants] WHERE name = '" + name + "'";

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

        public List<RestaurantListViewModel> GetRestaurantsByTags(List<string> tags)
        {
            List<RestaurantListViewModel> restaurants = new List<RestaurantListViewModel>();
            List<int> ids = new List<int>();
            SqlDataReader reader;
            SqlCommand command;

            int dbid;
            connection.Open();

            foreach (var tag in tags)
            {
                for (int i = 1; i <= 3; i++)
                {
                    dbid = -1;
                    string checkquery = "SELECT restaurant_id FROM [dbo].[Restaurant_Keywords] WHERE keyword" + i + " = '" + tag + "'";
                    command = new SqlCommand(checkquery, connection);

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        dbid = (int)reader["restaurant_id"];
                        ids.Add(dbid);
                    }
                    reader.Close();

                }
            }
            connection.Close();
            List<int> used = new List<int>();

            foreach(var id in ids)
            {
                if (!used.Contains(id))
                {
                    restaurants.Add(GetRestaurantListViewModelByID(id));
                    used.Add(id);
                }
            }

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

        public RestaurantListViewModel GetRestaurantListViewModelByID(int id)
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

            RestaurantStats stats = _statsRepository.GetStatsByRestaurantId(dbrestaurantid);
            List<string> tags = _statsRepository.GetTagsByRestaurantID(dbrestaurantid);
            

            return new RestaurantListViewModel(dbrestaurantid, dbname, dbaddress, dbcity, dbstate, dbzipcode, dbimage, stats, tags);
        }

            public Restaurant GetrestaurantByPhone(string phone)
        {
            throw new NotImplementedException();
        }

        public bool IsRestaurantRegistered(string email)
        {
            throw new NotImplementedException();
        }

        public bool InputRating(int restaurant_id, float rating, string review, DateTime time, int userId)
        {
            using (connection)
            {
                connection.Open();
                string sendqueryRestaurant = "INSERT INTO [dbo].[Restaurant_Ratings] VALUES ('" + review + "', '"
                                    + rating + "', '" + time + "', '"
                                    + userId + "', '" + restaurant_id + "')";


                string checkqueryTotalRatings = "SELECT count(*) FROM [dbo].[Restaurant_Ratings] WHERE restaurant_id = '" + restaurant_id + "'";

                string checkquerySumRatings = "SELECT SUM(rating) as rating from [dbo].[Restaurant_Ratings] WHERE restaurant_id = '" + restaurant_id + "'";

                int totalReviews = 0;
                double overallRating = 0.0, sumRatings = 0.0;

                using (SqlCommand command = new SqlCommand(sendqueryRestaurant, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(checkqueryTotalRatings, connection))
                {
                    totalReviews = (int)command.ExecuteScalar();
                }

                using (SqlCommand command = new SqlCommand(checkquerySumRatings, connection))
                {
                    sumRatings = (double)command.ExecuteScalar();
                }

                overallRating = (sumRatings / totalReviews);
                string sendqueryUpdateOverallRating = "UPDATE [dbo].[Restaurant_Stats] SET rating = '" + overallRating
                    + "', total_ratings = '" + totalReviews + "' WHERE restaurant_id = " + restaurant_id; ;

                using (SqlCommand command = new SqlCommand(sendqueryUpdateOverallRating, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return true;
        }

    }
}
