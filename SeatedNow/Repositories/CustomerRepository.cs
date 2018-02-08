﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SeatedNow.Models;

namespace SeatedNow.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        SqlConnection connection;

        public CustomerRepository()
        {
            IDataRepository dataRepo = new DataRepository();
            connection = dataRepo.GetDBConnection();
        }

        public void RegisterNewUser(CustomerAccount account)
        {

            try
            {
                using (connection)
                {
                    connection.Open();
                    string sendquery = "INSERT INTO [dbo].[Users] VALUES ('" + account.getFirstName() + "', '" + account.getLastName() + "', '" + account.Email + "', '" + account.Password + "', '" + account.PhoneNumber + "')";

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

        public void DeleteUser(CustomerAccount account)
        {
            throw new NotImplementedException();
        }

        public CustomerAccount GetUserByEmail(string email)
        {
            int dbuserid = -1;
            string dbname = "", dbphone="", dbemail="", dbpass="";
            string checkquery = "SELECT userid, firstname, lastname, phone, email, password FROM [dbo].[Users] WHERE email = '" + email + "'";

            connection.Open();
            SqlCommand command = new SqlCommand(checkquery, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbuserid = (int) reader["userid"];
                dbname = reader["firstname"] + " " + reader["lastname"];
                dbphone = reader["phone"].ToString();
                dbemail = reader["email"].ToString();
                dbpass = reader["password"].ToString();
            }

            connection.Close();

            return new CustomerAccount(dbuserid, dbname, dbemail, dbphone, dbpass);
        }

        public CustomerAccount GetUserByFirstLastName(string firstname, string lastname)
        {
            throw new NotImplementedException();
        }

        public CustomerAccount GetUserByID(int id)
        {
            throw new NotImplementedException();
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
