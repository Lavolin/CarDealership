using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly ICarService carService;

        public CarController(
            ICarService _carService)
        {
            carService = _carService;
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
        public async Task<IActionResult> Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(CarModel model)
        {
            int id = 1;

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new CarModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarModel model)
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
