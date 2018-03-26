using System;

namespace SeatedNow.Models
{
    public class RestaurantHours
    {

        public RestaurantHours()
        {

        }

        public RestaurantHours(int mondayOpen, int mondayClose, int tuesdayOpen, int tuesdayClose, int wednsedayOpen, int wednsedayClose, int thursdayOpen, int thursdayClose, int fridayOpen, int fridayClose, int saturdayOpen, int saturdayClose, int sundayOpen, int sundayClose)
        {
            MondayOpen = mondayOpen;
            MondayClose = mondayClose;
            TuesdayOpen = tuesdayOpen;
            TuesdayClose = tuesdayClose;
            WednsedayOpen = wednsedayOpen;
            WednsedayClose = wednsedayClose;
            ThursdayOpen = thursdayOpen;
            ThursdayClose = thursdayClose;
            FridayOpen = fridayOpen;
            FridayClose = fridayClose;
            SaturdayOpen = saturdayOpen;
            SaturdayClose = saturdayClose;
            SundayOpen = sundayOpen;
            SundayClose = sundayClose;
        }

        public int MondayOpen { get; set; }

        public int MondayClose { get; set; }

        public int TuesdayOpen { get; set; }

        public int TuesdayClose { get; set; }

        public int WednsedayOpen { get; set; }

        public int WednsedayClose { get; set; }

        public int ThursdayOpen { get; set; }

        public int ThursdayClose { get; set; }

        public int FridayOpen { get; set; }

        public int FridayClose { get; set; }

        public int SaturdayOpen { get; set; }

        public int SaturdayClose { get; set; }

        public int SundayOpen { get; set; }

        public int SundayClose { get; set; }

        public string ToAMPM(int Hours)
        {
            if (Hours == 25)
            {
                return ("Closed");
            }
            else if (Hours == 0)
            {
                return ("Midnight");
            }
            else if (Hours == 12)
            {
                return ("Noon");
            }
            else if(Hours > 12)
            {
                return (Hours - 12 + " pm");
            }
            else
            {
                return (Hours + " am");
            } 
        }

    }
}
