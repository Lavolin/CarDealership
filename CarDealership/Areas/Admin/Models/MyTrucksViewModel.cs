using CarDealership.Core.Models.Truck;

namespace CarDealership.Areas.Admin.Models
{
    public class MyTrucksViewModel
    {
        public IEnumerable<TruckServiceModel> AddedCars { get; set; } = new List<TruckServiceModel>();

        public IEnumerable<TruckServiceModel> SoldCars { get; set; } = new List<TruckServiceModel>();
    }
}
