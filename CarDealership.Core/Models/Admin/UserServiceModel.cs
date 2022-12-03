namespace CarDealership.Core.Models.Admin
{
    public class UserServiceModel
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null;
    }
}
