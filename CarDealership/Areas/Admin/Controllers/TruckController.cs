using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Areas.Admin.Controllers
{
    public class TruckController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
