using CarDealership.Core.Models.Truck;

namespace CarDealership.Models
{
    public class AllTrucksCountModel
    {
        public const int TrucksPerPage = 3;

        public string? Category { get; set; }

        public string? SearchTerm { get; set; }

        public TruckSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalTrucksCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<TruckServiceModel> Trucks { get; set; } = Enumerable.Empty<TruckServiceModel>();
    }
}
