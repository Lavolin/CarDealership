using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Motor;
using CarDealership.Infrastructure.Data.Common;

namespace CarDealership.Core.Services
{
    public class MotorService : IMotorService
    {
        private readonly IRepository repo;
        public MotorService(IRepository _repo) => repo = _repo;

        public Task<MotorCountModel> All(string? category = null, string? searchTerm = null, MotorSorting sorting = MotorSorting.Newest, int currentPage = 1, int carPerPage = 3)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MotorCategoryModel>> AllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> AllCategoriesNames()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MotorServiceModel>> AllMotorsByDealerId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MotorServiceModel>> AllMotorsByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task Buy(int motorId, string currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CategoryExists(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<int> Create(MotorModel model, int dealerId)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int motorId)
        {
            throw new NotImplementedException();
        }

        public Task Edit(int motorId, MotorModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMotorCategoryId(int motorId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasDealerWithId(int motorId, string currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsBought(int motorId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsBoughtByUserWithId(int motorId, string currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MotorHomeModel>> LastFiveMotors()
        {
            throw new NotImplementedException();
        }

        public Task<MotorDetailsModel> MotorDetailsById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Sell(int motorId)
        {
            throw new NotImplementedException();
        }
    }
}
