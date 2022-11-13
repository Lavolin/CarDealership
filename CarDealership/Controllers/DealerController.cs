using CarDealership.Core.Constants;
using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Dealer;
using CarDealership.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    [Authorize]
    public class DealerController : Controller
    {
        private readonly IDealerService dealerService;
        public DealerController(IDealerService _dealerService) => dealerService = _dealerService;        

        [HttpGet]
        public async Task<IActionResult> BeADealer()
        {
            if ((await dealerService.ExistsUserIdAsync(User.Id())))
            {
                TempData[MessageConstant.ErrorMessage] = "Вие в момента сте дилър";

                return RedirectToAction("Index", "Home");

            }

            TempData[MessageConstant.SuccessMessage] = "Вие в момента сте дилър";
            return RedirectToAction("Index", "Home");

            // return View();
        }

        [HttpPost]
        public async Task<IActionResult> BeADealer(BeADealerModel model)
        {
            return RedirectToAction("All", "Car");
        }
    }
}
