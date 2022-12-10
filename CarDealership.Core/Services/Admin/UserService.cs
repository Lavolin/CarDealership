using CarDealership.Core.Contracts.Admin;
using CarDealership.Core.Models.Admin;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Core.Services.Admin
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IRepository _repo, 
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<IEnumerable<UserServiceModel>> All()
        {
            List<UserServiceModel> result;

            result = await repo.AllReadonly<Dealer>()
                .Where(a => a.User.IsActive)
                .Select(d => new UserServiceModel()
                {
                    UserId = d.UserId,
                    Email = d.User.Email,                    
                    PhoneNumber = d.PhoneNumber
                })
                .ToListAsync();

            string[] dealerIds = result.Select(a => a.UserId).ToArray();

            result.AddRange(await repo.AllReadonly<ApplicationUser>()
                .Where(d => dealerIds.Contains(d.Id) == false)
                .Where(u => u.IsActive)
                .Select(d => new UserServiceModel()
                {
                    UserId = d.Id,
                    Email = d.Email,                    
                }).ToListAsync());

            return result;
        }

        public async Task<string> UserFullName(string userId)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            return $"{user?.FirstName} {user?.LastName}".Trim();
        }

        public async Task<bool> Clear(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            user.PhoneNumber = null;
            user.FirstName = null;
            user.Email = null;
            user.IsActive = false;
            user.LastName = null;
            user.NormalizedEmail = null;
            user.NormalizedUserName = null;
            user.PasswordHash = null;
            user.UserName = "UserIsDeleted";

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
