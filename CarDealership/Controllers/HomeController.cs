using CarDealership.Core.Contracts;
using CarDealership.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static CarDealership.Areas.Admin.Constants.AdminConstants;

namespace CarDealership.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarService carService;

        public HomeController(ICarService _carService) => carService = _carService;

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            var model = await carService.LastFiveCars();

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}