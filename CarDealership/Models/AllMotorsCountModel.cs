using CarDealership.Core.Models.Motor;

namespace CarDealership.Models
{
    public class AllMotorsCountModel
    {
        public const int MotorsPerPage = 3;

        public string? Category { get; set; }

        public string? SearchTerm { get; set; }

        public MotorSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalMotorsCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<MotorServiceModel> Motors { get; set; } = Enumerable.Empty<MotorServiceModel>();
    }
}
