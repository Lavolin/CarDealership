using CarDealership.Areas.Admin.Models;
using CarDealership.Core.Contracts;
using CarDealership.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Areas.Admin.Controllers
{
    public class MotorController : BaseController
    {
        private readonly IMotorService motorService;

        private readonly IDealerService dealerService;

        public MotorController(
            IMotorService _motorService, 
            IDealerService _dealerService)
        {
            motorService = _motorService;
            dealerService = _dealerService;
        }

        public async Task<IActionResult> Mine()
        {
            var myMotors = new MyMotorsViewModel();
            var adminId = User.Id();
            myMotors.SoldMotors = await motorService.AllMotorsByUserId(adminId);
            var dealerId = await dealerService.GetDealerId(adminId);
            myMotors.AddedMotors = await motorService.AllMotorsByDealerId(dealerId);

            return View(myMotors);
        }
    }
}
