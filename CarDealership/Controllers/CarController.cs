using CarDealership.Core.Models.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var model = new CarsViewModel();

            return View(model);
        }

        public async Task<IActionResult> Mine()
        {
            var model = new CarsViewModel();

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
        public async Task<IActionResult> Add(CarAddModel model)
        {
            int id = 1;

            return RedirectToAction(nameof(Details), new { id });
        } 

    }
}
