using CarDealership.Core.Models.Truck;

namespace CarDealership.Core.Contracts
{
    public interface ITruckService
    {
        Task<IEnumerable<TruckHomeModel>> LastFiveTrucks();

        Task<IEnumerable<TruckCategoryModel>> AllCategories();

        Task<bool> CategoryExists(int categoryId);

        Task<int> Create(TruckModel model, int dealerId);

        Task<TruckCountModel> All(
           string? category = null,
           string? searchTerm = null,
           TruckSorting sorting = TruckSorting.Newest,
           int currentPage = 1,
           int carPerPage = 3);

        Task<IEnumerable<string>> AllCategoriesNames();

        Task<IEnumerable<TruckServiceModel>> AllTrucksByDealerId(int id);

        Task<IEnumerable<TruckServiceModel>> AllTrucksByUserId(string userId);

        Task<TruckDetailsModel> TruckDetailsById(int id);

        Task<bool> Exists(int id);

        Task Edit(int truckId, TruckModel model);

        Task<bool> HasDealerWithId(int truckId, string currentUserId);

        Task<int> GetTruckCategoryId(int truckId);

        Task Delete(int truckId);

        Task<bool> IsBought(int truckId);

        Task<bool> IsBoughtByUserWithId(int truckId, string currentUserId);

        Task Buy(int truckId, string currentUserId);

        Task Sell(int truckId);
    }
}
