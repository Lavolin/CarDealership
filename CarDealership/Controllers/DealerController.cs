using CarDealership.Core.Models.Dealer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    [Authorize]
    public class DealerController : Controller
    {
        [HttpGet]
        public IActionResult BeADealer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BeADealer(BeADealerModel model)
        {
            return RedirectToAction("All", "Car");
        }
    }
}
