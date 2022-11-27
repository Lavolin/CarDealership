namespace CarDealership.Core.Models.Motor
{
    public class MotorCountModel
    {
        public int TotalMotorsCount { get; set; }

        public IEnumerable<MotorServiceModel> Motors { get; set; } = new List<MotorServiceModel>();
    }
}
