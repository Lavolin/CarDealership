using CarDealership.Areas.Admin.Models;
using CarDealership.Core.Contracts;
using CarDealership.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Areas.Admin.Controllers
{
    public class CarController : BaseController
    {
        private readonly ICarService carService;

        private readonly IDealerService dealerService;

        public CarController(
            ICarService _houseService,
            IDealerService _agentService)
        {
            carService = _houseService;
            dealerService = _agentService;
        }

        public async Task<IActionResult> Mine()
        {
            var myCars = new MyCarsViewModel();
            var adminId = User.Id();
            myCars.SoldCars = await carService.AllCarsByUserId(adminId);
            var dealerId = await dealerService.GetDealerId(adminId);
            myCars.AddedCars = await carService.AllCarsByDealerId(dealerId);

            return View(myCars);
        }
    }
}
