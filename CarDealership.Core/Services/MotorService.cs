using CarDealership.Core.Contracts;
using CarDealership.Infrastructure.Data.Common;

namespace CarDealership.Core.Services
{
    public class MotorService : IMotorService
    {
        private readonly IRepository repo;
        public MotorService(IRepository _repo) => repo = _repo;
       
    }
}
