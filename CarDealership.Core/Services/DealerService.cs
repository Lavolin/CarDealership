using CarDealership.Core.Contracts;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Core.Services
{
    public class DealerService : IDealerService
    {
        private readonly IRepository repo;
        public DealerService(IRepository _repo) => repo = _repo;

        public async Task Create(string userId, string phoneNumber)
        {
            var dealer = new Dealer()
            {
                UserId = userId,
                PhoneNumber = phoneNumber
            };

            await repo.AddAsync(dealer);
            await repo.SaveChangesAsync();
        }

        public async Task<bool> ExistsUserIdAsync(string userId)
        {
            return await repo.All<Dealer>()
                .AnyAsync(d => d.UserId == userId);
        }

        public async Task<bool> ExistUserPhoneAsync(string phoneNum)
        {
            return await repo.All<Dealer>()
                .AnyAsync(d => d.PhoneNumber == phoneNum);
        }

        public async Task<int> GetDealerId(string userId)
               => (await repo.AllReadonly<Dealer>()
                 .FirstOrDefaultAsync(d => d.UserId == userId))?.Id ?? 0;
        

        public async Task<bool> UserHasCars(string userId)
        {
            return await repo.All<Car>()
                 .AnyAsync(u => u.BuyerId == userId);
        } 
    }
}
