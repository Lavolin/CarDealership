namespace CarDealership.Core.Models.Truck
{
    public class TruckCountModel
    {
        public int TotalTrucksCount { get; set; }

        public IEnumerable<TruckServiceModel> Trucks { get; set; } = new List<TruckServiceModel>();
    }
}
