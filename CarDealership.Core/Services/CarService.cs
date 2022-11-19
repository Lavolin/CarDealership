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

        public async Task<IEnumerable<CarCategoryModel>> AllCategories()
        {
            return await repo.AllReadonly<CarCategory>()
                .OrderBy(c => c.Name)
                .Select(c => new CarCategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            return await repo.AllReadonly<CarCategory>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> Create(CarModel model, int dealerId)
        {
            var car = new Car()
            {
                Model = model.Model,
                CarCategoryId = model.CarCategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                DealerId = dealerId
            };

            await repo.AddAsync(car);
            await repo.SaveChangesAsync();

            return car.Id;
        }

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
