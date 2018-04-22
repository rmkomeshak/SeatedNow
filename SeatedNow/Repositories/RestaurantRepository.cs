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
        IReservationRepository _reservationRepository = new ReservationRepository();

        public RestaurantRepository()
        {
            IDataRepository dataRepo = new DataRepository();
            IStatsRepository statsRepo = new StatsRepository();
            IReservationRepository reservationRepo = new ReservationRepository();
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
                                    + restaurant.IsVerified + "', '" + restaurant.OwnerId + "' ,'"
                                    + restaurant.EventKey + "', '" + restaurant.Description + "', '"
                                    + restaurant.Color + "', '" + restaurant.Price + "', '"
                                    + restaurant.Website + "')";

                using (SqlCommand command = new SqlCommand(sendqueryRestaurant, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();

                int recentID = GetRecentRestaurantID();

                connection.Open();
                string sendqueryRestaurantTags = "INSERT INTO [dbo].[Restaurant_Keywords] VALUES ('" + recentID + "','" + restaurant.Keyword1 + "', '" + restaurant.Keyword2 + "', '" + restaurant.Keyword3 +"', '0')";

                using (SqlCommand command = new SqlCommand(sendqueryRestaurantTags, connection))
                {
                    command.ExecuteNonQuery();
                }


                string sendqueryRestaurants_Stats = "INSERT INTO [dbo].[Restaurant_Stats] VALUES ('" + recentID + "', '0', '0', '0', '0', '0')";

                using (SqlCommand command = new SqlCommand(sendqueryRestaurants_Stats, connection))
                {
                    command.ExecuteNonQuery();
                }

                string sendqueryRestaurants_Sections = "INSERT INTO [dbo].[Restaurant_Sections] VALUES (" + recentID + ", '', '', '', '', '', '')";

                using (SqlCommand command = new SqlCommand(sendqueryRestaurants_Sections, connection))
                {
                    command.ExecuteNonQuery();
                }


                connection.Close();
            }
        }

        public int GetRecentRestaurantID()
        {
            int recentID = -1;

            connection.Open();
            string sendquery = "SELECT TOP 1 id FROM [dbo].[Restaurants] ORDER BY id DESC";
            using (SqlCommand getidcommand = new SqlCommand(sendquery, connection))
            {

                SqlDataReader reader = getidcommand.ExecuteReader();

                while (reader.Read())
                {
                    recentID = (int)reader["id"];
                }
            }

            connection.Close();
            return recentID;
        }

        public void getRestaurantTags()
        {
            using (connection)
            {
                connection.Open();

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
                    + "', color = '" + restaurant.Color + "', price = '" + restaurant.Price
                    + "', website = '" + restaurant.Website + "' WHERE id = " + restaurant.Id;

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    UpdateTags(restaurant);
                    return true;
                }
            }

            
        }

        public bool UpdateTags(Restaurant restaurant)
        {
            string keyword1 = "", keyword2 = "", keyword3 = "";
            string[] tags = restaurant.Tags.ToArray();

            if (!string.IsNullOrEmpty(tags[0]))
            {
                keyword1 = tags[0];
            }

            if (!string.IsNullOrEmpty(tags[1]))
            {
                keyword2 = tags[1];
            }

            if (!string.IsNullOrEmpty(tags[2]))
            {
                keyword3 = tags[2];
            }



            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Restaurant_Keywords] SET keyword1 = '" + keyword1
                    + "', keyword2 = '" + keyword2 + "', keyword3 = '" + keyword3 + "' WHERE restaurant_id = " + restaurant.Id;

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

                _statsRepository.RefreshWaitTime(dbid);
            }

            connection.Close();
            return restaurants;
        }

        public List<RestaurantSection> GetSections(int restaurant_id)
        {
            List<RestaurantSection> sections = new List<RestaurantSection>();
            string section1 = "", section2 = "", section3 = "", section4 = "", section5 = "", section6 = "";
            string sendquery = "SELECT section1, section2, section3, section4, section5, section6 FROM [dbo].[Restaurant_Sections] WHERE restaurant_id = " + restaurant_id + ";";

            connection.Open();
            SqlCommand command = new SqlCommand(sendquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                section1 = reader["section1"].ToString();
                section2 = reader["section2"].ToString();
                section3 = reader["section3"].ToString();
                section4 = reader["section4"].ToString();
                section5 = reader["section5"].ToString();
                section6 = reader["section6"].ToString();
            }
            connection.Close();

            if (!string.IsNullOrEmpty(section1))
                sections.Add(new RestaurantSection(section1, 1));

            if (!string.IsNullOrEmpty(section2))
                sections.Add(new RestaurantSection(section2, 2));

            if (!string.IsNullOrEmpty(section3))
                sections.Add(new RestaurantSection(section3, 3));

            if (!string.IsNullOrEmpty(section4))
                sections.Add(new RestaurantSection(section4, 4));

            if (!string.IsNullOrEmpty(section5))
                sections.Add(new RestaurantSection(section5, 5));

            if (!string.IsNullOrEmpty(section6))
                sections.Add(new RestaurantSection(section6, 6));

            return sections;
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
            string checkquery = "SELECT id, name, address, city, state, zipcode, image FROM [dbo].[Restaurants] WHERE name LIKE '%" + name + "%'";

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

                Console.WriteLine("----------------------------------------------------" + dbid + "-" + dbname);
                Console.WriteLine("----------------------------------------------------" + restaurantStats.TotalRatings);
                Console.WriteLine("----------------------------------------------------" + tags[0] + tags[1] + tags[2]);

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

                Console.WriteLine("----------------------------------------------------" + dbid);
                Console.WriteLine("----------------------------------------------------" + restaurantStats.TotalRatings);
                Console.WriteLine("----------------------------------------------------" + tags[0] + tags[1] + tags[2]);

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

                Console.WriteLine("----------------------------------------------------" + dbid);
                Console.WriteLine("----------------------------------------------------" + restaurantStats.TotalRatings);
                Console.WriteLine("----------------------------------------------------" + tags[0] + tags[1] + tags[2]);


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
            int dbrestaurantid = -1, dbownerid = -1, dbprice = -1;
            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "", dbphone = "", dbeventkey = "", dbdescription = "", dbcolor = "", dbwebsite = "";
            bool dbverified = false;
            string checkquery = "SELECT * FROM [dbo].[Restaurants] WHERE id = '" + id + "'";

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
                dbcolor = reader["color"].ToString();
                dbprice = (int)reader["price"];
                dbwebsite = reader["website"].ToString();
        
            }

            connection.Close();

            return new Restaurant(dbrestaurantid, dbname, dbaddress, dbcity, dbzipcode, dbstate, dbphone, dbimage, dbverified, dbownerid, dbeventkey, dbdescription, dbcolor, dbwebsite, dbprice);
        }

        public Restaurant GetRestaurantByOwnerID(int id)
        {
            int dbrestaurantid = -1, dbownerid = -1, dbprice = -1;
            string dbname = "", dbaddress = "", dbcity = "", dbstate = "", dbzipcode = "", dbimage = "", dbphone = "", dbeventkey = "", dbdescription = "", dbcolor = "", dbwebsite = "";
            bool dbverified = false;
            string checkquery = "SELECT * FROM [dbo].[Restaurants] WHERE owner_id = '" + id + "'";

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
                dbcolor = reader["color"].ToString();
                dbprice = (int)reader["price"];
                dbwebsite = reader["website"].ToString();
            }

            connection.Close();

            return new Restaurant(dbrestaurantid, dbname, dbaddress, dbcity, dbzipcode, dbstate, dbphone, dbimage, dbverified, dbownerid, dbeventkey, dbdescription, dbcolor, dbwebsite, dbprice);
        }


        public List<RestaurantTableList> GetTablesByRestaurantID(int id)
        {
            List<RestaurantTableList> tablelist = new List<RestaurantTableList>();
            int dbrestaurantid = -1, dbreservationid = -1, dbtableid = -1, dbsection = -1;
            string dbtablename = "";
            bool dbtaken = false, dbenabled = false;

            string checkquery = "SELECT restaurant_id, table_name, taken, reservation_id, table_id, section, isEnabled FROM [dbo].[Restaurant_Tables] WHERE restaurant_id = '" + id + "'";

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
                dbsection = (int)reader["section"];
                dbenabled = (bool)reader["isEnabled"];
                tablelist.Add(new RestaurantTableList(dbrestaurantid, dbtablename, dbtaken, dbreservationid, dbtableid, dbsection, dbenabled));
            }

            connection.Close();

            return tablelist;
        }

        public int GetGuests(int restaurant_id, int table_id, string section_name)
        {
            int guests = -1;
            string query = "SELECT seats_reserved FROM [dbo].[Reservations] WHERE restaurant_id = " + restaurant_id + " AND table_id = " + table_id + " AND section = '" + section_name + "';";

            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                guests = (int)reader["seats_reserved"];
            }

            connection.Close();

            return guests;
        }

        public bool UpdateSections(int restaurant_id, string section1, string section2, string section3, string section4, string section5, string section6, List<RestaurantTableList> tables)
        {
            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Restaurant_Sections] SET section1 = '" + section1
                    + "', section2 = '" + section2 + "', section3 = '" + section3
                    + "', section4 = '" + section4 + "', section5 = '" + section5
                    + "', section6 = '" + section6 + "' WHERE restaurant_id = " + restaurant_id;

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    UpdateTables(restaurant_id, tables);
                    return true;
                }
            }
        }

        public bool UpdateTables(int restaurant_id, List<RestaurantTableList> tables)
        {
            using (connection)
            {
                connection.Open();
                string deletequery = "DELETE FROM [dbo].[Restaurant_Tables] WHERE restaurant_id = " + restaurant_id;

                using (SqlCommand command = new SqlCommand(deletequery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    InsertTables(tables);
                    return true;
                }
            }
        }

        public bool CreateMetricsTable(string name)
        {

            string table_name = name.Replace(" ", "_") + "_Metrics";

            using (connection)
            {
                connection.Open();
                string sendquery = "CREATE TABLE " + table_name + " ( restaurant_id int, searches int,  time DATETIME() );";

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }

        }
       
        public bool InsertTables(List<RestaurantTableList> tables)
        {
            using (connection)
            {
                connection.Open();
                foreach(var table in tables)
                {
                    string sendquery = "INSERT INTO [dbo].[Restaurant_Tables] VALUES (" + table.RestaurantId +
                        ", '" + table.TableName + "', '" + table.IsTaken + "', " + table.ReservationId + ", " + 
                        table.TableId + ", " + table.Section + ", '" + table.IsEnabled + "');";

                    using (SqlCommand command = new SqlCommand(sendquery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
                return true;
            }
        }

        public bool OccupyTable(int restaurant_id, string table_name, DateTime time)
        {
            bool b = true;
            int reservation_id = _reservationRepository.GetReservationID(time);

            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Restaurant_Tables] SET taken = '" + b + "', reservation_id = " + reservation_id + " WHERE restaurant_id = " + restaurant_id + " AND table_name = '" + table_name + "';";

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
        }

        public bool OccupyTable(int restaurant_id, int table_id)
        {
            bool b = true;

            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Restaurant_Tables] SET taken = '" + b + "' WHERE restaurant_id = " + restaurant_id + " AND table_id = " + table_id + ";";

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
        }

        public bool FreeTable(int restaurant_id, string table_name)
        {
            bool b = false;

            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Restaurant_Tables] SET taken = '" + b + "', reservation_id = -1 WHERE restaurant_id = " + restaurant_id + " AND table_name = '" + table_name + "';";

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
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
