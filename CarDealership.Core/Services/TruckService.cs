using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Truck;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Core.Services
{
    public class TruckService : ITruckService
    {
        private readonly IRepository repo;
        public TruckService(IRepository _repo) => repo = _repo;

        public async Task<TruckCountModel> All(
            string? category = null,
            string? searchTerm = null,
            TruckSorting sorting = TruckSorting.Newest,
            int currentPage = 1, int truckPerPage = 3)
        {
            var result = new TruckCountModel();

            var trucks = repo.AllReadonly<Truck>()
                .Where(c => c.IsActive);

            if (string.IsNullOrEmpty(category) == false)
            {
                trucks = trucks
                    .Where(c => c.TruckCategory.Name == category);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                trucks = trucks
                    .Where(c => EF.Functions.Like(c.Model.ToLower(), searchTerm) ||
                        EF.Functions.Like(c.Price.ToString().ToLower(), searchTerm) ||
                        EF.Functions.Like(c.Description.ToLower(), searchTerm));
            }

            trucks = sorting switch
            {
                TruckSorting.Price => trucks
                    .OrderBy(c => c.Price),
                TruckSorting.Newest => trucks
                    .OrderBy(c => c.DealerId),
                _ => trucks.OrderByDescending(c => c.Id)
            };

            result.Trucks = await trucks
                .Skip((currentPage - 1) * truckPerPage)
                .Take(truckPerPage)
                .Select(c => new TruckServiceModel()
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    IsBought = c.BuyerId != null,
                    Price = c.Price,
                    Model = c.Model
                })
                .ToListAsync();

            result.TotalTrucksCount = await trucks.CountAsync();

            return result;
        }

        public async Task<IEnumerable<TruckServiceModel>> AllTrucksByDealerId(int id)
        {
            return await repo.AllReadonly<Truck>()
                .Where(c => c.IsActive)
                .Where(d => d.DealerId == id)
                .Select(c => new TruckServiceModel()
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    IsBought = c.BuyerId != null,
                    Price = c.Price,
                    Model = c.Model
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TruckServiceModel>> AllTrucksByUserId(string userId)
        {
            return await repo.AllReadonly<Truck>()
                 .Where(c => c.IsActive)
                 .Where(d => d.BuyerId == userId)
                 .Select(c => new TruckServiceModel()
                 {
                     Id = c.Id,
                     ImageUrl = c.ImageUrl,
                     IsBought = c.BuyerId != null,
                     Price = c.Price,
                     Model = c.Model
                 })
                 .ToListAsync();
        }

        public async Task<IEnumerable<TruckCategoryModel>> AllCategories()
        {
            return await repo.AllReadonly<TruckCategory>()
                .OrderBy(c => c.Name)
                .Select(c => new TruckCategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await repo.AllReadonly<TruckCategory>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task Buy(int truckId, string currentUserId)
        {
            var truck = await repo.GetByIdAsync<Truck>(truckId);

            if (truck != null && truck.BuyerId != null)
            {
                throw new ArgumentException("The truck is bought");
            }

            truck.BuyerId = currentUserId;

            await repo.SaveChangesAsync();
        }

        public async Task<TruckDetailsModel> TruckDetailsById(int id)
        {
            return await repo.AllReadonly<Truck>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == id)
                .Select(c => new TruckDetailsModel()
                {
                    Category = c.TruckCategory.Name,
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
            return await repo.AllReadonly<TruckCategory>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> Create(TruckModel model, int dealerId)
        {
            var truck = new Truck()
            {
                Model = model.Model,
                TruckCategoryId = model.TruckCategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                DealerId = dealerId
            };

            await repo.AddAsync(truck);
            await repo.SaveChangesAsync();

            return truck.Id;
        }

        public async Task Delete(int truckId)
        {
            var truck = await repo.GetByIdAsync<Truck>(truckId);
            truck.IsActive = false;

            await repo.SaveChangesAsync();
        }

        public async Task Edit(int truckId, TruckModel model)
        {
            var truck = await repo.GetByIdAsync<Truck>(truckId);

            truck.Description = model.Description;
            truck.ImageUrl = model.ImageUrl;
            truck.Price = model.Price;
            truck.Model = model.Model;
            truck.TruckCategoryId = model.TruckCategoryId;

            await repo.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
            => await repo.AllReadonly<Truck>()
                         .AnyAsync(c => c.Id == id && c.IsActive);

        public async Task<int> GetTruckCategoryId(int truckId)
            => (await repo.GetByIdAsync<Truck>(truckId)).TruckCategoryId;

        public async Task<bool> HasDealerWithId(int truckId, string currentUserId)
        {
            bool result = false;
            var truck = await repo.AllReadonly<Truck>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == truckId)
                .Include(c => c.Dealer)
                .FirstOrDefaultAsync();

            if (truck?.Dealer != null && truck.Dealer.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> IsBought(int truckId)
        {
            return (await repo.GetByIdAsync<Truck>(truckId)).BuyerId != null;
        }

        public async Task<bool> IsBoughtByUserWithId(int truckId, string currentUserId)
        {
            bool result = false;
            var truck = await repo.AllReadonly<Truck>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == truckId)
                .FirstOrDefaultAsync();

            if (truck != null && truck.BuyerId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<TruckHomeModel>> LastFiveTrucks()
        {
            return await repo.AllReadonly<Truck>()
                 .Where(c => c.IsActive)
                 .OrderByDescending(c => c.Id)
                 .Select(c => new TruckHomeModel()
                 {
                     Id = c.Id,
                     ImageUrl = c.ImageUrl,
                     Model = c.Model
                 })
                 .Take(5)
                 .ToListAsync();
        }

        public async Task Sell(int truckId)
        {
            var truck = await repo.GetByIdAsync<Truck>(truckId);

            truck.BuyerId = null;

            await repo.SaveChangesAsync();
        }
    }
}
