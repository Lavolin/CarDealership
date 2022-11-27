using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Motor;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Core.Services
{
    public class MotorService : IMotorService
    {
        private readonly IRepository repo;
        public MotorService(IRepository _repo) => repo = _repo;

        public async Task<MotorCountModel> All(
            string? category = null,
            string? searchTerm = null,
            MotorSorting sorting = MotorSorting.Newest,
            int currentPage = 1, int motorPerPage = 3)
        {
            var result = new MotorCountModel();

            var motors = repo.AllReadonly<Motor>()
                .Where(c => c.IsActive);

            if (string.IsNullOrEmpty(category) == false)
            {
                motors = motors
                    .Where(h => h.MotorCategory.Name == category);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                motors = motors
                    .Where(c => EF.Functions.Like(c.Model.ToLower(), searchTerm) ||
                        EF.Functions.Like(c.Price.ToString().ToLower(), searchTerm) ||
                        EF.Functions.Like(c.Description.ToLower(), searchTerm));
            }

            motors = sorting switch
            {
                MotorSorting.Price => motors
                    .OrderBy(c => c.Price),
                MotorSorting.Newest => motors
                    .OrderBy(c => c.DealerId),
                _ => motors.OrderByDescending(c => c.Id)
            };

            result.Motors = await motors
                .Skip((currentPage - 1) * motorPerPage)
                .Take(motorPerPage)
                .Select(c => new MotorServiceModel()
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    IsBought = c.BuyerId != null,
                    Price = c.Price,
                    Model = c.Model
                })
                .ToListAsync();

            result.TotalMotorsCount = await motors.CountAsync();

            return result;
        }

        public async Task<IEnumerable<MotorServiceModel>> AllMotorsByDealerId(int id)
        {
            return await repo.AllReadonly<Motor>()
                .Where(c => c.IsActive)
                .Where(d => d.DealerId == id)
                .Select(c => new MotorServiceModel()
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    IsBought = c.BuyerId != null,
                    Price = c.Price,
                    Model = c.Model
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MotorServiceModel>> AllMotorsByUserId(string userId)
        {
            return await repo.AllReadonly<Motor>()
                 .Where(c => c.IsActive)
                 .Where(d => d.BuyerId == userId)
                 .Select(c => new MotorServiceModel()
                 {
                     Id = c.Id,
                     ImageUrl = c.ImageUrl,
                     IsBought = c.BuyerId != null,
                     Price = c.Price,
                     Model = c.Model
                 })
                 .ToListAsync();
        }

        public async Task<IEnumerable<MotorCategoryModel>> AllCategories()
        {
            return await repo.AllReadonly<MotorCategory>()
                .OrderBy(c => c.Name)
                .Select(c => new MotorCategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await repo.AllReadonly<MotorCategory>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task Buy(int motorId, string currentUserId)
        {
            var motor = await repo.GetByIdAsync<Motor>(motorId);

            if (motor != null && motor.BuyerId != null)
            {
                throw new ArgumentException("The motor is bought");
            }

            motor.BuyerId = currentUserId;

            await repo.SaveChangesAsync();
        }

        public async Task<MotorDetailsModel> MotorDetailsById(int id)
        {
            return await repo.AllReadonly<Motor>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == id)
                .Select(c => new MotorDetailsModel()
                {
                    Category = c.MotorCategory.Name,
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
            return await repo.AllReadonly<MotorCategory>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> Create(MotorModel model, int dealerId)
        {
            var motor = new Motor()
            {
                Model = model.Model,
                MotorCategoryId = model.MotorCategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                DealerId = dealerId
            };

            await repo.AddAsync(motor);
            await repo.SaveChangesAsync();

            return motor.Id;
        }

        public async Task Delete(int motorId)
        {
            var motor = await repo.GetByIdAsync<Motor>(motorId);
            motor.IsActive = false;

            await repo.SaveChangesAsync();
        }

        public async Task Edit(int motorId, MotorModel model)
        {
            var motor = await repo.GetByIdAsync<Motor>(motorId);

            motor.Description = model.Description;
            motor.ImageUrl = model.ImageUrl;
            motor.Price = model.Price;
            motor.Model = model.Model;
            motor.MotorCategoryId = model.MotorCategoryId;

            await repo.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
            => await repo.AllReadonly<Motor>()
                         .AnyAsync(c => c.Id == id && c.IsActive);

        public async Task<int> GetMotorCategoryId(int motorId)
            => (await repo.GetByIdAsync<Motor>(motorId)).MotorCategoryId;

        public async Task<bool> HasDealerWithId(int motorId, string currentUserId)
        {
            bool result = false;
            var motor = await repo.AllReadonly<Motor>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == motorId)
                .Include(c => c.Dealer)
                .FirstOrDefaultAsync();

            if (motor?.Dealer != null && motor.Dealer.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> IsBought(int motorId)
        {
            return (await repo.GetByIdAsync<Motor>(motorId)).BuyerId != null;
        }

        public async Task<bool> IsBoughtByUserWithId(int motorId, string currentUserId)
        {
            bool result = false;
            var motor = await repo.AllReadonly<Motor>()
                .Where(c => c.IsActive)
                .Where(c => c.Id == motorId)
                .FirstOrDefaultAsync();

            if (motor != null && motor.BuyerId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<MotorHomeModel>> LastFiveMotors()
        {
            return await repo.AllReadonly<Motor>()
                 .Where(c => c.IsActive)
                 .OrderByDescending(c => c.Id)
                 .Select(c => new MotorHomeModel()
                 {
                     Id = c.Id,
                     ImageUrl = c.ImageUrl,
                     Model = c.Model
                 })
                 .Take(5)
                 .ToListAsync();
        }

        public async Task Sell(int motorId)
        {
            var motor = await repo.GetByIdAsync<Motor>(motorId);

            motor.BuyerId = null;

            await repo.SaveChangesAsync();
        }
    }
}
