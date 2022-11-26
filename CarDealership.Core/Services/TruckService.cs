using CarDealership.Core.Contracts;
using CarDealership.Infrastructure.Data.Common;

namespace CarDealership.Core.Services
{
    public class TruckService : ITruckService
    {
        private readonly IRepository repo;
        public TruckService(IRepository _repo) => repo = _repo;
       

    }
}
