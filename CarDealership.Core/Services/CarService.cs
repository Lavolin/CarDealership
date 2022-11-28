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

        public async Task<CarCountModel> All(
            string? category = null, 
            string? searchTerm = null,
            CarSorting sorting = CarSorting.Newest,
            int currentPage = 1, int carPerPage = 3)
        {
            var result = new CarCountModel();

            var cars = repo.AllReadonly<Car>()
                .Where(c => c.IsActive);

            if (string.IsNullOrEmpty(category) == false)
            {
                cars = cars
                    .Where(c => c.CarCategory.Name == category);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                cars = cars
                    .Where(c => EF.Functions.Like(c.Model.ToLower(), searchTerm) ||
                        EF.Functions.Like(c.Price.ToString().ToLower(), searchTerm) ||
                        EF.Functions.Like(c.Description.ToLower(), searchTerm));
            }

            cars = sorting switch
            {
                CarSorting.Price => cars
                    .OrderBy(c => c.Price),
                CarSorting.Newest => cars
                    .OrderBy(c => c.DealerId),
                _ => cars.OrderByDescending(c => c.Id)
            };

            result.Cars = await cars
                .Skip((currentPage - 1) * carPerPage)
                .Take(carPerPage)
                .Select(c => new CarServiceModel()
                {                    
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    IsBought = c. BuyerId!= null,
                    Price = c.Price,
                    Model = c.Model
                })
                .ToListAsync();

            result.TotalCarsCount = await cars.CountAsync();

            return result;
        }

        public async Task<IEnumerable<CarServiceModel>> AllCarsByDealerId(int id)
        {
            return await repo.AllReadonly<Car>()
                .Where(c => c.IsActive)
                .Where(d => d.DealerId == id)
                .Select(c => new CarServiceModel()
                {
                    Id= c.Id,
                    ImageUrl= c.ImageUrl,
                    IsBought = c.BuyerId != null,
                    Price = c.Price,
                    Model = c.Model
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CarServiceModel>> AllCarsByUserId(string userId)
        {
            return await repo.AllReadonly<Car>()
                 .Where(c => c.IsActive)
                 .Where(d => d.BuyerId == userId)
                 .Select(c => new CarServiceModel()
                 {
                     Id = c.Id,
                     ImageUrl = c.ImageUrl,
                     IsBought = c.BuyerId != null,
                     Price = c.Price,
                     Model = c.Model
                 })
                 .ToListAsync();
        }

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

        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await repo.AllReadonly<CarCategory>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task Buy(int carId, string currentUserId)
        {
            var car = await repo.GetByIdAsync<Car>(carId);

            if (car != null && car.BuyerId != null)
            {
                throw new ArgumentException("The car is bought");
            }
           
            car.BuyerId = currentUserId;            

            await repo.SaveChangesAsync();
        }

        public async Task<CarDetailsModel> CarDetailsById(int id)
        {
            return await repo.AllReadonly<Car>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == id)
                .Select(c => new CarDetailsModel()
                {                    
                    Category = c.CarCategory.Name,
                    Description = c.Description,
                    Id = id,
                    ImageUrl = c.ImageUrl,
                    IsBought = c.BuyerId != null,
                    Price = c.Price,
                    Model = c.Model,
                    Dealer = new Models.Dealer.DealerServiceModel()
                    {
                        Email = c.Dealer.User.Email,
                        PhoneNumber = c.Dealer.PhoneNumber
                    }

                })
                .FirstAsync();
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

        public async Task Delete(int carId)
        {
            var car = await repo.GetByIdAsync<Car>(carId);
            car.IsActive = false;

            await repo.SaveChangesAsync();
        }

        public async Task Edit(int carId, CarModel model)
        {
            var car = await repo.GetByIdAsync<Car>(carId);

            car.Description = model.Description;
            car.ImageUrl = model.ImageUrl;
            car.Price = model.Price;
            car.Model = model.Model;            
            car.CarCategoryId = model.CarCategoryId;

            await repo.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)         
            => await repo.AllReadonly<Car>()
                         .AnyAsync(c => c.Id == id && c.IsActive);

        public async Task<int> GetCarCategoryId(int carId)
            => (await repo.GetByIdAsync<Car>(carId)).CarCategoryId;

        public async Task<bool> HasDealerWithId(int carId, string currentUserId)
        {
            bool result = false;
            var car = await repo.AllReadonly<Car>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == carId)
                .Include(c => c.Dealer)
                .FirstOrDefaultAsync();

            if (car?.Dealer != null && car.Dealer.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> IsBought(int carId)
        {
            return (await repo.GetByIdAsync<Car>(carId)).BuyerId != null;
        }

        public async Task<bool> IsBoughtByUserWithId(int carId, string currentUserId)
        {
            bool result = false;
            var car = await repo.AllReadonly<Car>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == carId)               
                .FirstOrDefaultAsync();

            if (car != null && car.BuyerId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<CarHomeModel>> LastFiveCars()
        {
            return await repo.AllReadonly<Car>()
                 .Where(c => c.IsActive)
                 .OrderByDescending(c => c.Id)
                 .Select(c => new CarHomeModel() 
                 {
                     Id = c.Id,
                     ImageUrl= c.ImageUrl,
                     Model = c.Model
                 })
                 .Take(5)
                 .ToListAsync();
        }

        public async Task Sell(int carId)
        {
            var car = await repo.GetByIdAsync<Car>(carId);
           
            car.BuyerId = null;

            await repo.SaveChangesAsync();
        }
    }
}
