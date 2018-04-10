using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SeatedNow.Models;

namespace SeatedNow.Repositories
{
    public class StatsRepository : IStatsRepository
    {

        SqlConnection connection;

        public StatsRepository()
        {
            IDataRepository dataRepo = new DataRepository();
            connection = dataRepo.GetDBConnection();
        }

        public bool UpdateStats(RestaurantStats stats)
        {
            using (connection)
            {
                connection.Open();
                string sendquery = "UPDATE [dbo].[Restaurant_Stats] SET reservations = '" + stats.Reservations
                    + "', cur_customers = '" + stats.Customers + "', wait_time '" + stats.WaitTime
                    + "' WHERE restaurant_id = " + stats.RestaurantId;

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
        }

        public RestaurantStats GetStatsByRestaurantId(int id)
        {
            int dbrestaurantid = -1, dbreservations = -1, dbcustomers = -1, dbwaittime = -1, dbtotalratings = -1;
            double dbrating = 0.0;
            string checkquery = "SELECT restaurant_id, reservations, cur_customers, wait_time, rating, total_ratings FROM [dbo].[Restaurant_Stats] WHERE restaurant_id = '" + id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbrestaurantid = (int)reader["restaurant_id"];
                dbreservations = (int)reader["reservations"];
                dbcustomers = (int)reader["cur_customers"];
                dbwaittime = (int)reader["wait_time"];
                dbrating = (double)reader["rating"];
                dbtotalratings = (int)reader["total_ratings"];
            }

            connection.Close();

            return new RestaurantStats(dbrestaurantid, dbreservations, dbcustomers, dbwaittime, dbrating, dbtotalratings);
        }

        public List<string> GetTagsByRestaurantName(string name)
        {
            int dbrestaurantid = -1;
            string checkquery = "SELECT id FROM [dbo].[Restaurants] WHERE name LIKE '%" + name + "%'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbrestaurantid = (int)reader["id"];
            }

            connection.Close();

            return GetTagsByRestaurantID(dbrestaurantid);
        }

        public List<string> GetTagsByRestaurantID(int id)
        {
            List<string> tags = new List<string>();
            int dbrestaurantid = -1;
            string dbtag1 = "", dbtag2 = "", dbtag3 = "";
            string checkquery = "SELECT restaurant_id, keyword1, keyword2, keyword3 FROM [dbo].[Restaurant_Keywords] WHERE restaurant_id = '" + id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbrestaurantid = (int)reader["restaurant_id"];
                dbtag1 = reader["keyword1"].ToString();
                dbtag2 = reader["keyword2"].ToString();
                dbtag3 = reader["keyword3"].ToString();
            }

            tags.Add(dbtag1);
            tags.Add(dbtag2);
            tags.Add(dbtag3);

            connection.Close();

            return tags;
        }

        public RestaurantHours GetHoursByRestaurantId(int Id)
        {

            RestaurantHours hours = new RestaurantHours();

            int dbopen = -1, dbclose = -1, dbday = -1;
            string checkquery = "SELECT hours_day, hours_open, hours_close FROM [dbo].[Restaurant_Hours] WHERE restaurant_id = '" + Id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                RestaurantHours DefaultHours = new RestaurantHours(9, 21, 9, 21, 9, 21, 9, 21, 9, 21, 9, 21, 9, 21);
                return DefaultHours;
            }

            while (reader.Read())
            {
                dbday = (int)reader["hours_day"];
                dbopen = (int)reader["hours_open"];
                dbclose = (int)reader["hours_close"];

                switch (dbday)
                {
                    case 0:
                        hours.MondayOpen = dbopen;
                        hours.MondayClose = dbclose;
                        break;
                    case 1:
                        hours.TuesdayOpen = dbopen;
                        hours.TuesdayClose = dbclose;
                        break;
                    case 2:
                        hours.WednsedayOpen = dbopen;
                        hours.WednsedayClose = dbclose;
                        break;
                    case 3:
                        hours.ThursdayOpen = dbopen;
                        hours.ThursdayClose = dbclose;
                        break;
                    case 4:
                        hours.FridayOpen = dbopen;
                        hours.FridayClose = dbclose;
                        break;
                    case 5:
                        hours.SaturdayOpen = dbopen;
                        hours.SaturdayClose = dbclose;
                        break;
                    case 6:
                        hours.SundayOpen = dbopen;
                        hours.SundayClose = dbclose;
                        break;
                }
            }

            connection.Close();
            return hours;
        }

        public bool SetHoursByRestaurantId(int Id, RestaurantHours Hours)
        {

            if (DoHoursExistOnDay(Id, 0))
            {
                string sendquery = "UPDATE [dbo].[Restaurant_Hours] SET hours_open = '" + Hours.MondayOpen + "', hours_close = '" + Hours.MondayClose
                    + "' WHERE restaurant_id = " + Id + " AND hours_day = 0";

                connection.Open();

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } else
            {
                string sendquery = "INSERT INTO [dbo].[Restaurant_Hours] VALUES ('" + Id + "', '0', '" + Hours.MondayOpen + "', '" + Hours.MondayClose + "')";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (DoHoursExistOnDay(Id, 1))
            {
                string sendquery = "UPDATE [dbo].[Restaurant_Hours] SET hours_open = '" + Hours.TuesdayOpen + "', hours_close = '" + Hours.TuesdayClose
                    + "' WHERE restaurant_id = " + Id + " AND hours_day = 1";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                string sendquery = "INSERT INTO [dbo].[Restaurant_Hours] VALUES ('" + Id + "', '1', '" + Hours.TuesdayOpen + "', '" + Hours.TuesdayClose + "')";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (DoHoursExistOnDay(Id, 2))
            {
                string sendquery = "UPDATE [dbo].[Restaurant_Hours] SET hours_open = '" + Hours.WednsedayOpen + "', hours_close = '" + Hours.WednsedayClose
                    + "' WHERE restaurant_id = " + Id + " AND hours_day = 2";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                string sendquery = "INSERT INTO [dbo].[Restaurant_Hours] VALUES ('" + Id + "', '2', '" + Hours.WednsedayOpen + "', '" + Hours.WednsedayClose + "')";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (DoHoursExistOnDay(Id, 3))
            {
                string sendquery = "UPDATE [dbo].[Restaurant_Hours] SET hours_open = '" + Hours.ThursdayOpen + "', hours_close = '" + Hours.ThursdayClose
                    + "' WHERE restaurant_id = " + Id + " AND hours_day = 3";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                string sendquery = "INSERT INTO [dbo].[Restaurant_Hours] VALUES ('" + Id + "', '3', '" + Hours.ThursdayOpen + "', '" + Hours.ThursdayClose + "')";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (DoHoursExistOnDay(Id, 4))
            {
                string sendquery = "UPDATE [dbo].[Restaurant_Hours] SET hours_open = '" + Hours.FridayOpen + "', hours_close = '" + Hours.FridayClose
                    + "' WHERE restaurant_id = " + Id + " AND hours_day = 4";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                string sendquery = "INSERT INTO [dbo].[Restaurant_Hours] VALUES ('" + Id + "', '4', '" + Hours.FridayOpen + "', '" + Hours.FridayClose + "')";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (DoHoursExistOnDay(Id, 5))
            {
                string sendquery = "UPDATE [dbo].[Restaurant_Hours] SET hours_open = '" + Hours.SaturdayOpen + "', hours_close = '" + Hours.SaturdayClose
                    + "' WHERE restaurant_id = " + Id + " AND hours_day = 5";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                string sendquery = "INSERT INTO [dbo].[Restaurant_Hours] VALUES ('" + Id + "', '5', '" + Hours.SaturdayOpen + "', '" + Hours.SaturdayClose + "')";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (DoHoursExistOnDay(Id, 6))
            {
                string sendquery = "UPDATE [dbo].[Restaurant_Hours] SET hours_open = '" + Hours.SundayOpen + "', hours_close = '" + Hours.SundayClose
                    + "' WHERE restaurant_id = " + Id + " AND hours_day = 6";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                string sendquery = "INSERT INTO [dbo].[Restaurant_Hours] VALUES ('" + Id + "', '6', '" + Hours.SundayOpen + "', '" + Hours.SundayClose + "')";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return true;
        }

        public List<RestaurantRatings> GetRatingsByRestaurantId(int Id)
        {
            List<RestaurantRatings> ratings = new List<RestaurantRatings>();

            string dbcomment = "";
            double dbrating = 0.0;
            string checkquery = "SELECT comment, rating FROM [dbo].[Restaurant_Ratings] WHERE restaurant_id = '" + Id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbcomment = reader["comment"].ToString();
                dbrating = (double)reader["rating"];

                ratings.Add(new RestaurantRatings(dbrating, dbcomment));
            }

            connection.Close();
            return ratings;
        }

        public Boolean DoHoursExistOnDay(int RestaurantId, int Day)
        {
            string checkquery = "SELECT count(*) FROM [dbo].[Restaurant_Hours] WHERE restaurant_id = '" + RestaurantId + "' AND hours_day = '" + Day + "'";
            int rows;

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            rows = (int)command.ExecuteScalar();

            connection.Close();

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public void RefreshWaitTime(int RestaurantId)
        {

            int openTables = 0;
            string opentablesquery = "SELECT count(*) FROM [dbo].[Restaurant_Tables] WHERE restaurant_id = '" + RestaurantId + "' AND taken='false'";

            connection.Open();
            using (SqlCommand command = new SqlCommand(opentablesquery, connection))
            {
                openTables = (int)command.ExecuteScalar();
                connection.Close();
            }

            if (openTables > 0)
            {
                SetWaitTime(RestaurantId, 0);
            } else
            {
                int estimatedWait = 0;
                int numReservations = GetNumReservations(RestaurantId);


                estimatedWait = (int) (7.2 * numReservations);

                SetWaitTime(RestaurantId, estimatedWait);

            }
        }

        public void SetWaitTime(int RestaurantId, int Minutes)
        {
            string sendquery = "UPDATE [dbo].[Restaurant_Stats] SET wait_time = '" + Minutes + "' WHERE restaurant_id = '" + RestaurantId + "'";

            connection.Open();
            using (SqlCommand command = new SqlCommand(sendquery, connection))
            {
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public int GetNumReservations(int RestaurantId)
        {

            int numReservations = 0;
            string getnumquery = "SELECT count(*) FROM [dbo].[Reservations] WHERE restaurant_id = '" + RestaurantId + "'";

            connection.Open();
            using (SqlCommand command = new SqlCommand(getnumquery, connection))
            {
                numReservations = (int)command.ExecuteScalar();
                connection.Close();
            }

            return numReservations;
        }

    }
}
