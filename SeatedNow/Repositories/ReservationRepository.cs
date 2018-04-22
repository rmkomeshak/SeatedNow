using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SeatedNow.Models;

namespace SeatedNow.Repositories
{
    public class ReservationRepository : IReservationRepository
    {

        SqlConnection connection;
        IStatsRepository _statsRepository = new StatsRepository();

        public ReservationRepository()
        {
            IDataRepository dataRepo = new DataRepository();
            connection = dataRepo.GetDBConnection();
        }

        public List<DiningReservation> GetReservationsByCustomerID(int id, int results)
        {

            List<DiningReservation> reservations = new List<DiningReservation>();

            int dbreservationid = -1, dbrestaurantid = -1, dbaccountid = -1, dbseatsreserved = -1, dbtableid = -1;
            bool dbinuse = false;
            DateTime dbreservationdatetime;
            string dbsection = "";
            string checkquery;

            if (results == -1)
            {
                checkquery = "SELECT reservation_id, restaurant_id, account_id, seats_reserved, reservation_datetime, table_id, section, in_use FROM [dbo].[Reservations] WHERE account_id = '" + id + "' limit " + results;
            }
            else
            {
                checkquery = "SELECT reservation_id, restaurant_id, account_id, seats_reserved, reservation_datetime, table_id, section, in_use FROM [dbo].[Reservations] WHERE account_id = '" + id + "'";
            }

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
                dbinuse = (bool)reader["in_use"];
                reservations.Add(new DiningReservation(dbreservationid, dbrestaurantid, dbaccountid, dbseatsreserved, dbreservationdatetime, dbtableid, dbsection, dbinuse));
            }

            connection.Close();

            return reservations;
        }

        public List<DiningReservation> GetReservationsByRestaurantId(int id)
        {

            List<DiningReservation> reservations = new List<DiningReservation>();

            int dbreservationid = -1, dbrestaurantid = -1, dbaccountid = -1, dbseatsreserved = -1, dbtableid = -1;
            bool dbinuse = false;
            DateTime dbreservationdatetime = DateTime.MinValue;
            string dbsection = "";
            string checkquery;
            checkquery = "SELECT reservation_id, restaurant_id, account_id, seats_reserved, reservation_datetime, table_id, section, in_use FROM [dbo].[Reservations] WHERE restaurant_id = " + id + " AND in_use = 'true';";

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
                dbinuse = (bool)reader["in_use"];
                reservations.Add(new DiningReservation(dbreservationid, dbrestaurantid, dbaccountid, dbseatsreserved, dbreservationdatetime, dbtableid, dbsection, dbinuse));
            }

            connection.Close();

            return reservations;
        }

        public void CreateReservation(DiningReservation reservation)
        {

            using (connection)
            {
                connection.Open();
                string sendquery = "INSERT INTO [dbo].[Reservations] VALUES ('" + reservation.RestaurantID + "', '" + reservation.OwnerID + "', '" + reservation.Guests + "', '" + reservation.Time + "', '" + reservation.TableID + "', '" + reservation.Section + "', 'true')";

                using (SqlCommand command = new SqlCommand(sendquery, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            Console.WriteLine("----------------------------------Running RUNNING FRESH------------------------------------");
            _statsRepository.RefreshWaitTime(reservation.RestaurantID);
            
        }

        public int GetReservationID(DateTime time)
        {
            int id = -1;
            string sendquery = "SELECT reservation_id FROM [dbo].[Reservations] WHERE reservation_datetime = '" + time + "';";
            connection.Open();
            SqlCommand command = new SqlCommand(sendquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                id = (int)reader["reservation_id"];
            }

            connection.Close();

            return id;
        }

        public void DeleteReservation(int reservation_id)
        {
            connection.Open();
            string sendquery = "DELETE FROM [dbo].[Reservations] WHERE reservation_id = " + reservation_id + ";";

            using (SqlCommand command = new SqlCommand(sendquery, connection))
            {
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        public void DeleteReservation(int restaurant_id, int table_id, string section_name)
        {
            connection.Open();
            string sendquery = "DELETE FROM [dbo].[Reservations] WHERE restaurant_id = " + restaurant_id + " AND table_id = " + table_id + " AND section = '" + section_name + "';";

            using (SqlCommand command = new SqlCommand(sendquery, connection))
            {
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        public void DisableReservation(int restaurant_id, int table_id, string section_name)
        {
            connection.Open();
            string sendquery = "UPDATE [dbo].[Reservations] SET in_use = 'false' WHERE restaurant_id = " + restaurant_id + " AND table_id = " + table_id + " AND section = '" + section_name + "';";

            using (SqlCommand command = new SqlCommand(sendquery, connection))
            {
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


    }
}
