using CarDealership.Core.Models.Dealer;

namespace CarDealership.Core.Models.Car
{
    public class CarDetailsModel : CarServiceModel
    {
        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public DealerServiceModel Dealer { get; set; }
    }
}
