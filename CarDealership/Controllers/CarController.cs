using CarDealership.Core.Constants;
using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Car;
using CarDealership.Extensions;
using CarDealership.Models;
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
                TempData[MessageConstant.ErrorMessage] = "Car does not exist";

                return RedirectToAction(nameof(All));

            }

            var model = await carService.CarDetailsById(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!await dealerService.ExistsUserIdAsync(User.Id()))
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
            if (!await dealerService.ExistsUserIdAsync(User.Id()))
            {
                return RedirectToAction(nameof(DealerController.BeADealer), "Dealer");
            }

            if (!await carService.CategoryExists(carModel.CarCategoryId))
            {
                TempData[MessageConstant.ErrorMessage] = "Car category does not exist";                
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
            if (!await carService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await carService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var car = await carService.CarDetailsById(id);
            var categoryId = await carService.GetCarCategoryId(id);

            var model = new CarModel()
            {
                Id = id,                
                CarCategoryId = categoryId,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                Price = car.Price,
                Model = car.Model,
                CarCategories = await carService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarModel carModel)
        {
            if (id != carModel.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (!await carService.Exists(carModel.Id))
            {
                TempData[MessageConstant.ErrorMessage] = "Car does not exist";
                
                carModel.CarCategories = await carService.AllCategories();

                return View(carModel);
            }

            if (!await carService.HasDealerWithId(carModel.Id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (!await carService.CategoryExists(carModel.CarCategoryId))
            {                
                TempData[MessageConstant.ErrorMessage] = "Category does not exist";

                carModel.CarCategories = await carService.AllCategories();

                return View(carModel);
            }

            if (!ModelState.IsValid)
            {
                carModel.CarCategories = await carService.AllCategories();
                return View(carModel);
            }

            await carService.Edit(carModel.Id, carModel);

            return RedirectToAction(nameof(Details), new { carModel.Id });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await carService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await carService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var car = await carService.CarDetailsById(id);

            var model = new CarDetailsViewModel()
            {
                Model = car.Model,
                ImageUrl = car.ImageUrl,                
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CarDetailsViewModel carModel)
        {
            if (!await carService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await carService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await carService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            if (!await carService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            //if (!await dealerService.ExistsUserIdAsync(User.Id()))
            //{
            //    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            //}

            if (await carService.IsBought(id))
            {
                return RedirectToAction(nameof(All));
            }

            await carService.Buy(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Sell(int id)
        {
            if (!await carService.Exists(id) ||
                !await carService.IsBought(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await carService.IsBoughtByUserWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await carService.Sell(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}
