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

    }
}
