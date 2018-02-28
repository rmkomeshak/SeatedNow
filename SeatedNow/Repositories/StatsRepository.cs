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
                    + "', restaurant_id = '" + stats.RestaurantId
                    + "' WHERE id = " + stats.Id;

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
            int dbrestaurantid = -1, dbreservations = -1, dbcustomers = -1, dbwaittime = -1, dbid = -1;
            string checkquery = "SELECT id, reservations, cur_customers, wait_time, restaurant_id FROM [dbo].[Restaurant_Stats] WHERE restaurant_id = '" + id + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbid = (int)reader["id"];
                dbreservations = (int)reader["reservations"];
                dbcustomers = (int)reader["cur_customers"];
                dbwaittime = (int)reader["wait_time"];
                dbrestaurantid = (int)reader["restaurant_id"];
            }

            connection.Close();

            return new RestaurantStats(dbid, dbreservations, dbcustomers, dbwaittime, dbrestaurantid);
        }


    }
}
