using CarDealership.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    public class MotorController : Controller
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

    }
}
