using CarDealership.Core.Constants;
using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Motor;
using CarDealership.Extensions;
using CarDealership.Models;
using Microsoft.AspNetCore.Authorization;
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


        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllMotorsCountModel allMotors)
        {
            var result = await motorService.All(
            allMotors.Category,
            allMotors.SearchTerm,
            allMotors.Sorting,
            allMotors.CurrentPage,
            AllMotorsCountModel.MotorsPerPage);

            allMotors.TotalMotorsCount = result.TotalMotorsCount;
            allMotors.Categories = await motorService.AllCategoriesNames();
            allMotors.Motors = result.Motors;

            return View(allMotors);
        }

        public async Task<IActionResult> Mine()
        {
            IEnumerable<MotorServiceModel> myMotors;

            var userId = User.Id();

            if (await dealerService.ExistsUserIdAsync(userId))
            {
                int dealerId = await dealerService.GetDealerId(userId);
                myMotors = await motorService.AllMotorsByDealerId(dealerId);
            }
            else
            {
                myMotors = await motorService.AllMotorsByUserId(userId);
            }

            return View(myMotors);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (!await motorService.Exists(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Motor does not exist";

                return RedirectToAction(nameof(All));

            }

            var model = await motorService.MotorDetailsById(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!await dealerService.ExistsUserIdAsync(User.Id()))
            {
                return RedirectToAction(nameof(DealerController.BeADealer), "Dealer");
            }

            var model = new MotorModel()
            {
                MotorCategories = await motorService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MotorModel motorModel)
        {
            if (!await dealerService.ExistsUserIdAsync(User.Id()))
            {
                return RedirectToAction(nameof(DealerController.BeADealer), "Dealer");
            }

            if (!await motorService.CategoryExists(motorModel.MotorCategoryId))
            {
                TempData[MessageConstant.ErrorMessage] = "Motor category does not exist";
            }

            if (!ModelState.IsValid)
            {
                motorModel.MotorCategories = await motorService.AllCategories();

                return View(motorModel);
            }

            int dealerId = await dealerService.GetDealerId(User.Id());

            int id = await motorService.Create(motorModel, dealerId);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await motorService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await motorService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var motor = await motorService.MotorDetailsById(id);
            var categoryId = await motorService.GetMotorCategoryId(id);

            var model = new MotorModel()
            {
                Id = id,
                MotorCategoryId = categoryId,
                Description = motor.Description,
                ImageUrl = motor.ImageUrl,
                Price = motor.Price,
                Model = motor.Model,
                MotorCategories = await motorService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MotorModel motorModel)
        {
            if (id != motorModel.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (!await motorService.Exists(motorModel.Id))
            {
                TempData[MessageConstant.ErrorMessage] = "Motor does not exist";

                motorModel.MotorCategories = await motorService.AllCategories();

                return View(motorModel);
            }

            if (!await motorService.HasDealerWithId(motorModel.Id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (!await motorService.CategoryExists(motorModel.MotorCategoryId))
            {
                TempData[MessageConstant.ErrorMessage] = "Category does not exist";

                motorModel.MotorCategories = await motorService.AllCategories();

                return View(motorModel);
            }

            if (!ModelState.IsValid)
            {
                motorModel.MotorCategories = await motorService.AllCategories();
                return View(motorModel);
            }

            await motorService.Edit(motorModel.Id, motorModel);

            return RedirectToAction(nameof(Details), new { motorModel.Id });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await motorService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await motorService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var motor = await motorService.MotorDetailsById(id);

            var model = new MotorDetailsViewModel()
            {
                Model = motor.Model,
                ImageUrl = motor.ImageUrl,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, MotorDetailsViewModel motorModel)
        {
            if (!await motorService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await motorService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await motorService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            if (!await motorService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            //if (!await dealerService.ExistsUserIdAsync(User.Id()))
            //{
            //    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            //}

            if (await motorService.IsBought(id))
            {
                return RedirectToAction(nameof(All));
            }

            await motorService.Buy(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Sell(int id)
        {
            if (!await motorService.Exists(id) ||
                !await motorService.IsBought(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await motorService.IsBoughtByUserWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await motorService.Sell(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}
