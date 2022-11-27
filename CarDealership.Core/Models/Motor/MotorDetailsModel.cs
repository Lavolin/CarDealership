using CarDealership.Core.Models.Dealer;

namespace CarDealership.Core.Models.Motor
{
    public class MotorDetailsModel : MotorServiceModel
    {
        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public DealerServiceModel Dealer { get; set; }
    }
}
