using CarDealership.Core.Models.Truck;

namespace CarDealership.Areas.Admin.Models
{
    public class MyTrucksViewModel
    {
        public IEnumerable<TruckServiceModel> AddedTrucks { get; set; } = new List<TruckServiceModel>();

        public IEnumerable<TruckServiceModel> SoldTrucks { get; set; } = new List<TruckServiceModel>();
    }
}
