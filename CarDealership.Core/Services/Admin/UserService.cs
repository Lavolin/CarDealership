using CarDealership.Core.Contracts.Admin;
using CarDealership.Core.Models.Admin;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Core.Services.Admin
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;

        public UserService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<IEnumerable<UserServiceModel>> All()
        {
            List<UserServiceModel> result;

            result = await repo.AllReadonly<Dealer>()
                .Select(d => new UserServiceModel()
                {
                    UserId = d.UserId,
                    Email = d.User.Email,
                    FullName = $"{d.User.FirstName} {d.User.LastName}",
                    PhoneNumber = d.PhoneNumber
                })
                .ToListAsync();

            string[] dealerIds = result.Select(a => a.UserId).ToArray();

            result.AddRange(await repo.AllReadonly<ApplicationUser>()
                .Where(d => dealerIds.Contains(d.Id) == false)
                .Select(d => new UserServiceModel()
                {
                    UserId = d.Id,
                    Email = d.Email,
                    FullName = $"{d.FirstName} {d.LastName}"
                }).ToListAsync());

            return result;
        }

        public async Task<string> UserFullName(string userId)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            return $"{user?.FirstName} {user?.LastName}".Trim();
        }
    }
}
