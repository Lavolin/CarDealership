using CarDealership.Core.Models.Motor;

namespace CarDealership.Core.Contracts
{
    public interface IMotorService
    {
        Task<IEnumerable<MotorHomeModel>> LastFiveMotors();

        Task<IEnumerable<MotorCategoryModel>> AllCategories();

        Task<bool> CategoryExists(int categoryId);

        Task<int> Create(MotorModel model, int dealerId);

        Task<MotorCountModel> All(
           string? category = null,
           string? searchTerm = null,
           MotorSorting sorting = MotorSorting.Newest,
           int currentPage = 1,
           int carPerPage = 3);

        Task<IEnumerable<string>> AllCategoriesNames();

        Task<IEnumerable<MotorServiceModel>> AllMotorsByDealerId(int id);

        Task<IEnumerable<MotorServiceModel>> AllMotorsByUserId(string userId);

        Task<MotorDetailsModel> MotorDetailsById(int id);

        Task<bool> Exists(int id);

        Task Edit(int motorId, MotorModel model);

        Task<bool> HasDealerWithId(int motorId, string currentUserId);

        Task<int> GetMotorCategoryId(int motorId);

        Task Delete(int motorId);

        Task<bool> IsBought(int motorId);

        Task<bool> IsBoughtByUserWithId(int motorId, string currentUserId);

        Task Buy(int motorId, string currentUserId);

        Task Sell(int motorId);
    }
}
