using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class RestaurantTableList
    {

        public RestaurantTableList(int restaurant_id, string table_name, bool taken, int reservation_id, int table_id)
        {
            RestaurantId = restaurant_id;
            TableName = table_name;
            IsTaken = taken;
            TableId = table_id;
            ReservationId = reservation_id;
        }

        public int RestaurantId { get; set; }
        public string TableName { get; set; }
        public bool IsTaken { get; set; }
        public int TableId { get; set; }
        public int ReservationId { get; set; }


    }
}
