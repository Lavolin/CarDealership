using CarDealership.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    public class TruckController : Controller
    {
        private readonly ITruckService truckService;
        private readonly IDealerService dealerService;
        public TruckController(
            ITruckService _truckService, 
            IDealerService _dealerService)
        {
            truckService = _truckService;
            dealerService = _dealerService;
        }

    }
}
