using CarDealership.Areas.Admin.Models;
using CarDealership.Core.Contracts;
using CarDealership.Core.Services;
using CarDealership.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Areas.Admin.Controllers
{
    public class TruckController : BaseController
    {
        private readonly ITruckService truckService;

        private readonly IDealerService dealerService;

        public TruckController(ITruckService _truckService, IDealerService _dealerService)
        {
            truckService = _truckService;
            dealerService = _dealerService;
        }

        public async Task<IActionResult> Mine()
        {
            var myTrucks = new MyTrucksViewModel();
            var adminId = User.Id();
            myTrucks.SoldTrucks = await truckService.AllTrucksByUserId(adminId);
            var dealerId = await dealerService.GetDealerId(adminId);
            myTrucks.AddedTrucks = await truckService.AllTrucksByDealerId(dealerId);

            return View(myTrucks);
        }
    }
}
