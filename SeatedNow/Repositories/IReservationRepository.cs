using SeatedNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    interface IReservationRepository
    {
        List<DiningReservation> GetReservationsByCustomerID(int CustomerId, int results);
        void CreateReservation(DiningReservation reservation);
        void DeleteReservation(int reservation_id);
        void DisableReservation(int restaurant_id, int table_id, string section_name);
        void DeleteReservation(int restaurant_id, int table_id, string section_name);
        int GetReservationID(DateTime time);
        List<DiningReservation> GetReservationsByRestaurantId(int id);

    }
}
