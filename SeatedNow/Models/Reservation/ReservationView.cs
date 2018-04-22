using System;

namespace SeatedNow.Models
{
    public class ReservationView
    {

        public ReservationView(string name, string table_name, int guests)
        {
            Name = name;
            TableName = table_name;
            Guests = guests;
        }

        string Name { get; set; }
        string TableName { get; set; }
        int Guests { get; set; }
    }
}
