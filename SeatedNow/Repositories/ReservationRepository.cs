﻿using System;
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

        public ReservationRepository()
        {
            IDataRepository dataRepo = new DataRepository();
            connection = dataRepo.GetDBConnection();
        }

        public List<DiningReservation> GetReservationsByCustomerID(int id, int results)
        {

            List<DiningReservation> reservations = new List<DiningReservation>();

            int dbreservationid = -1, dbrestaurantid = -1, dbaccountid = -1, dbseatsreserved = -1, dbtableid = -1;
            DateTime dbreservationdatetime;
            string dbsection = "";
            string checkquery;

            if (results == -1)
            {
                checkquery = "SELECT reservation_id, restaurant_id, account_id, seats_reserved, reservation_datetime, table_id, section FROM [dbo].[Reservations] WHERE account_id = '" + id + "' limit " + results;
            }
            else
            {
                checkquery = "SELECT reservation_id, restaurant_id, account_id, seats_reserved, reservation_datetime, table_id, section FROM [dbo].[Reservations] WHERE account_id = '" + id + "'";
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
                reservations.Add(new DiningReservation(dbreservationid, dbrestaurantid, dbaccountid, dbseatsreserved, dbreservationdatetime, dbtableid, dbsection));
            }

            connection.Close();

            return reservations;
        }


    }
}