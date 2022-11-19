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

            var model = new BeADealerModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BeADealer(BeADealerModel model)
        {
            var userId = User.Id();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await dealerService.ExistsUserIdAsync(userId))
            {
                TempData[MessageConstant.ErrorMessage] = "Вие в момента сте дилър";

                return RedirectToAction("Index", "Home");
            }

            if (await dealerService.ExistUserPhoneAsync(model.Phone))
            {
                TempData[MessageConstant.ErrorMessage] = "Телефона ви вече е използван";

                return RedirectToAction("Index", "Home");
            }

            await dealerService.Create(userId, model.Phone);

            return RedirectToAction("All", "Car");
        }
    }
}
