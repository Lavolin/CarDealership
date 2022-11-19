namespace CarDealership.Core.Models.Car
{
    public class CarCountModel
    {
        public int TotalCarsCount { get; set; }

        public IEnumerable<CarServiceModel> Cars { get; set; } = new List<CarServiceModel>();
    }
}
