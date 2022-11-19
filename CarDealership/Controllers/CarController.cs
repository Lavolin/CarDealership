using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Car;
using CarDealership.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly ICarService carService;
        private readonly IDealerService dealerService;


        public CarController(
            ICarService _carService, 
            IDealerService _dealerService)
        {
            carService = _carService;
            dealerService = _dealerService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var model = new CarModel();

            return View(model);
        }

        public async Task<IActionResult> Mine()
        {
            var model = new CarModel();

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var model = new CarDetailsModel();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if ((await dealerService.ExistsUserIdAsync(User.Id())) == false)
            {
                return RedirectToAction(nameof(DealerController.BeADealer), "Dealer");
            }

            var model = new CarModel()
            {
                CarCategories = await carService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarModel carModel)
        {
            if ((await dealerService.ExistsUserIdAsync(User.Id())) == false)
            {
                return RedirectToAction(nameof(DealerController.BeADealer), "Dealer");
            }

            if ((await carService.CategoryExists(carModel.CarCategoryId)) == false)
            {
                ModelState.AddModelError(nameof(carModel.CarCategoryId), "Car Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                carModel.CarCategories = await carService.AllCategories();

                return View(carModel);
            }

            int dealerId = await dealerService.GetDealerId(User.Id());

            int id = await carService.Create(carModel, dealerId);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new CarModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarModel carModel)
        {
            return RedirectToAction(nameof(Details), new { id });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return RedirectToAction(nameof(All));

        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            return RedirectToAction(nameof(Mine));

        }

        [HttpPost]
        public async Task<IActionResult> Sell(int id)
        {
            return RedirectToAction(nameof(Mine));

        }
    }
}
