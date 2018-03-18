using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class SearchContent
    {
        public SearchContent(string query, List<RestaurantListViewModel> rl)
        {
            Query = query;
            Results = rl;
        }

        public List<RestaurantListViewModel> Results { get; set; }
        public string Query { get; set; }
    }
}
