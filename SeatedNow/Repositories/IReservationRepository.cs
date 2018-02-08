using SeatedNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    interface IReservationRepository
    {
        DiningReservation GetReservationsByCustomer(int CustomerId);
        List<DiningReservation> getReservationsByRestaurant(int RestaurantId);

    }
}
