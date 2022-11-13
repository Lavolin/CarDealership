using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Car;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Core.Services
{
    public class CarService : ICarService
    {
        private readonly IRepository repo;

        public CarService(IRepository _repo) => repo = _repo;

        public async Task<IEnumerable<CarHomeModel>> LastThreeCars()
        {
            return await repo.AllReadonly<Car>()
                 .OrderByDescending(c => c.Id)
                 .Select(c => new CarHomeModel() 
                 {
                     Id = c.Id,
                     ImageUrl= c.ImageUrl,
                     Model = c.Model
                 })
                 .Take(3)
                 .ToListAsync();
        }
    }
}
