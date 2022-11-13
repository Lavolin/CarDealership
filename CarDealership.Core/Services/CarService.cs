using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Car;

namespace CarDealership.Core.Services
{
    public class CarService : ICarService
    {
        public Task<IEnumerable<CarHomeModel>> LastThreeCars()
        {
            throw new NotImplementedException();
        }
    }
}
