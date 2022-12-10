using CarDealership.Core.Models.Admin;

namespace CarDealership.Core.Contracts.Admin
{
    public interface IUserService
    {
        Task<string> UserFullName(string userId);

        Task<IEnumerable<UserServiceModel>> All();

        Task<bool> Clear(string userId);
    }
}
