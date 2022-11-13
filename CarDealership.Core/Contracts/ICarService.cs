using CarDealership.Core.Models.Car;

namespace CarDealership.Core.Contracts
{
    public interface ICarService
    {
        Task<IEnumerable<CarHomeModel>> LastThreeCars();

    }
}
