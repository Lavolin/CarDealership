using CarDealership.Core.Models.Car;

namespace CarDealership.Areas.Admin.Models
{
    public class MyCarsViewModel
    {
        public IEnumerable<CarServiceModel> AddedCars { get; set; } = new List<CarServiceModel>();

        public IEnumerable<CarServiceModel> SoldCars { get; set; } = new List<CarServiceModel>();
    }
}
