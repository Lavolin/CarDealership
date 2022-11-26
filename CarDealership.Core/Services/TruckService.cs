using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Truck;
using CarDealership.Infrastructure.Data.Common;

namespace CarDealership.Core.Services
{
    public class TruckService : ITruckService
    {
        private readonly IRepository repo;
        public TruckService(IRepository _repo) => repo = _repo;

        public Task<TruckCountModel> All(string? category = null, string? searchTerm = null, TruckSorting sorting = TruckSorting.Newest, int currentPage = 1, int carPerPage = 3)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TruckCategoryModel>> AllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> AllCategoriesNames()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TruckServiceModel>> AllTrucksByDealerId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TruckServiceModel>> AllTrucksByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task Buy(int truckId, string currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CategoryExists(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<int> Create(TruckModel model, int dealerId)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int truckId)
        {
            throw new NotImplementedException();
        }

        public Task Edit(int truckId, TruckModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTruckCategoryId(int truckId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasDealerWithId(int truckId, string currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsBought(int truckId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsBoughtByUserWithId(int truckId, string currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TruckHomeModel>> LastFiveTrucks()
        {
            throw new NotImplementedException();
        }

        public Task Sell(int truckId)
        {
            throw new NotImplementedException();
        }

        public Task<TruckDetailsModel> TruckDetailsById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
