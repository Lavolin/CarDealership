using CarDealership.Core.Models.Dealer;

namespace CarDealership.Core.Models.Truck
{
    public class TruckDetailsModel
    {
        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public DealerServiceModel Dealer { get; set; }
    }
}
