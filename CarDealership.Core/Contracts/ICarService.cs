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
    }
}
