using CarDealership.Core.Models.Motor;

namespace CarDealership.Areas.Admin.Models
{
    public class MyMotorsViewModel
    {
        public IEnumerable<MotorServiceModel> AddedMotors { get; set; } = new List<MotorServiceModel>();

        public IEnumerable<MotorServiceModel> SoldMotors { get; set; } = new List<MotorServiceModel>();
    }
}
