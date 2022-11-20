using CarDealership.Core.Models.Car;

namespace CarDealership.Core.Contracts
{
    public interface ICarService
    {
        Task<IEnumerable<CarHomeModel>> LastThreeCars();

        Task<IEnumerable<CarCategoryModel>> AllCategories();

        Task<bool> CategoryExists(int categoryId);

        Task<int> Create(CarModel model, int dealerId);

        Task<CarCountModel> All(
           string? category = null,
           string? searchTerm = null,
           CarSorting sorting = CarSorting.Newest,
           int currentPage = 1,
           int carPerPage = 3);

        Task<IEnumerable<string>> AllCategoriesNames();

        Task<IEnumerable<CarServiceModel>> AllCarsByDealerId(int id);

        Task<IEnumerable<CarServiceModel>> AllCarsByUserId(string userId);

        Task<CarDetailsModel> CarDetailsById(int id);

        Task<bool> Exists(int id);

        Task Edit(int carId, CarModel model);

        Task<bool> HasDealerWithId(int carId, string currentUserId);

        Task<int> GetCarCategoryId(int carId);

        Task Delete(int carId);

        Task<bool> IsBought(int carId);

        Task<bool> IsBoughtByUserWithId(int carId, string currentUserId);

        Task Buy(int carId, string currentUserId);

        Task Sell(int carId);
    }
}
