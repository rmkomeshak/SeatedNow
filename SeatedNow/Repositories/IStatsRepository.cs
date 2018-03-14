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
        List<RestaurantRatings> GetRatingsByRestaurantId(int Id);
    }
}
