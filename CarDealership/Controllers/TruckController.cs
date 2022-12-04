using CarDealership.Core.Constants;
using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Truck;
using CarDealership.Extensions;
using CarDealership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CarDealership.Areas.Admin.Constants.AdminConstants;

namespace CarDealership.Controllers
{
    [Authorize]
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllTrucksCountModel allTrucks)
        {
            var result = await truckService.All(
            allTrucks.Category,
            allTrucks.SearchTerm,
            allTrucks.Sorting,
            allTrucks.CurrentPage,
            AllTrucksCountModel.TrucksPerPage);

            allTrucks.TotalTrucksCount = result.TotalTrucksCount;
            allTrucks.Categories = await truckService.AllCategoriesNames();
            allTrucks.Trucks = result.Trucks;

            return View(allTrucks);
        }

        public async Task<IActionResult> Mine()
        {
            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction("Mine", "Truck", new { area = AreaName });
            }

            IEnumerable<TruckServiceModel> myTrucks;

            var userId = User.Id();

            if (await dealerService.ExistsUserIdAsync(userId))
            {
                int dealerId = await dealerService.GetDealerId(userId);
                myTrucks = await truckService.AllTrucksByDealerId(dealerId);
            }
            else
            {
                myTrucks = await truckService.AllTrucksByUserId(userId);
            }

            return View(myTrucks);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (!await truckService.Exists(id))
            {
                TempData[MessageConstant.ErrorMessage] = "Truck does not exist";

                return RedirectToAction(nameof(All));

            }

            var model = await truckService.TruckDetailsById(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!await dealerService.ExistsUserIdAsync(User.Id()))
            {
                return RedirectToAction(nameof(DealerController.BeADealer), "Dealer");
            }

            var model = new TruckModel()
            {
                TruckCategories = await truckService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TruckModel truckModel)
        {
            if (!await dealerService.ExistsUserIdAsync(User.Id()))
            {
                return RedirectToAction(nameof(DealerController.BeADealer), "Dealer");
            }

            if (!await truckService.CategoryExists(truckModel.TruckCategoryId))
            {
                TempData[MessageConstant.ErrorMessage] = "Truck category does not exist";
            }

            if (!ModelState.IsValid)
            {
                truckModel.TruckCategories = await truckService.AllCategories();

                return View(truckModel);
            }

            int dealerId = await dealerService.GetDealerId(User.Id());

            int id = await truckService.Create(truckModel, dealerId);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await truckService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await truckService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var truck = await truckService.TruckDetailsById(id);
            var categoryId = await truckService.GetTruckCategoryId(id);

            var model = new TruckModel()
            {
                Id = id,
                TruckCategoryId = categoryId,
                Description = truck.Description,
                ImageUrl = truck.ImageUrl,
                Price = truck.Price,
                Model = truck.Model,
                TruckCategories = await truckService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TruckModel truckModel)
        {
            if (id != truckModel.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (!await truckService.Exists(truckModel.Id))
            {
                TempData[MessageConstant.ErrorMessage] = "Truck does not exist";

                truckModel.TruckCategories = await truckService.AllCategories();

                return View(truckModel);
            }

            if (!await truckService.HasDealerWithId(truckModel.Id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (!await truckService.CategoryExists(truckModel.TruckCategoryId))
            {
                TempData[MessageConstant.ErrorMessage] = "Category does not exist";

                truckModel.TruckCategories = await truckService.AllCategories();

                return View(truckModel);
            }

            if (!ModelState.IsValid)
            {
                truckModel.TruckCategories = await truckService.AllCategories();
                return View(truckModel);
            }

            await truckService.Edit(truckModel.Id, truckModel);

            return RedirectToAction(nameof(Details), new { truckModel.Id });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await truckService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await truckService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var truck = await truckService.TruckDetailsById(id);

            var model = new TruckDetailsViewModel()
            {
                Model = truck.Model,
                ImageUrl = truck.ImageUrl,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, TruckDetailsViewModel truckModel)
        {
            if (!await truckService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await truckService.HasDealerWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await truckService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            if (!await truckService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            //if (!await dealerService.ExistsUserIdAsync(User.Id()))
            //{
            //    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            //}

            if (await truckService.IsBought(id))
            {
                return RedirectToAction(nameof(All));
            }

            await truckService.Buy(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Sell(int id)
        {
            if (!await truckService.Exists(id) ||
                !await truckService.IsBought(id))
            {
                return RedirectToAction(nameof(All));
            }

            if (!await truckService.IsBoughtByUserWithId(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await truckService.Sell(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}
