namespace CarDealership.Core.Contracts
{
    public interface IDealerService
    {
        Task<bool> ExistsUserIdAsync(string userId);

        Task<bool> ExistUserPhoneAsync(string phoneNum);

        Task<bool> UserHasCars(string userId);

        Task Create(string userId, string phoneNumber);

        Task<int> GetDealerId(string userId);

    }
}
