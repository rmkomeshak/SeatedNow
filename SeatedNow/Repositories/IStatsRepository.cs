using SeatedNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    interface IStatsRepository
    {
        bool UpdateStats(RestaurantStats stats);
        RestaurantStats GetStatsByRestaurantId(int id);
        List<string> GetTagsByRestaurantID(int id);
        RestaurantHours GetHoursByRestaurantId(int Id);
        bool SetHoursByRestaurantId(int Id, RestaurantHours Hours);
        List<RestaurantRatings> GetRatingsByRestaurantId(int Id);
        List<string> GetTagsByRestaurantName(string Name);
        void RefreshWaitTime(int RestaurantId);
        void SetWaitTime(int RestaurantId, int Minutes);
        int GetNumReservations(int RestaurantId);
        bool UpdateReservations();
    }
}
