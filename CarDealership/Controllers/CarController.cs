using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Car;
using CarDealership.Extensions;
using CarDealership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllCarsCountModel allCars)
        {
            var result = await carService.All(
            allCars.Category,
            allCars.SearchTerm,
            allCars.Sorting,
            allCars.CurrentPage,
            AllCarsCountModel.CarsPerPage);

            allCars.TotalCarsCount = result.TotalCarsCount;
            allCars.Categories = await carService.AllCategoriesNames();
            allCars.Cars = result.Cars;

            return View(allCars);
        }

        public async Task<IActionResult> Mine()
        {
            IEnumerable<CarServiceModel> myCars;

            var userId = User.Id();

            if (await dealerService.ExistsUserIdAsync(userId))
            {
                int dealerId = await dealerService.GetDealerId(userId);
                myCars = await carService.AllCarsByDealerId(dealerId);
            }
            else
            {
                myCars = await carService.AllCarsByUserId(userId);
            }

            return View(myCars);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (!await carService.Exists(id))
            {
                ModelState.AddModelError(nameof(carService.Exists), "Car does not exists");
                return RedirectToAction(nameof(All));
            }

            var model = await carService.CarDetailsById(id);

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
