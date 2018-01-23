using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    public class DataRepository
    {
        public readonly SqlConnection connection;
        //Create the readonly configuration items for the database connection string

        public DataRepository()
        {

            this.connection = new SqlConnection();
            connection.ConnectionString = "Server=tcp:seatednow.database.windows.net,1433;Initial Catalog=seatednow;Persist Security Info=False;User ID=seatednow;Password=Sipawd123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        public SqlConnection getDBConnection()
        {
            return this.connection;
        }

    }
}
