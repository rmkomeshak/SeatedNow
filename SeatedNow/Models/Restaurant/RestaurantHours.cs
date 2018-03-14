using System;

namespace SeatedNow.Models
{
    public class RestaurantHours
    {
        public TimeSpan MondayOpen { get; set; }

        public TimeSpan MondayClose { get; set; }

        public DateTime TuesdayOpen { get; set; }

        public DateTime TuesdayClose { get; set; }

        public DateTime WednsedayOpen { get; set; }

        public DateTime WednsedayClose { get; set; }

        public DateTime ThursdayOpen { get; set; }

        public DateTime ThursdayClose { get; set; }

        public DateTime FridayOpen { get; set; }

        public DateTime FridayClose { get; set; }

        public DateTime SaturdayOpen { get; set; }

        public DateTime SaturdayClose { get; set; }

        public DateTime SundayOpen { get; set; }

        public DateTime SundayClose { get; set; }

    }
}
