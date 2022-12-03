using CarDealership.Core.Contracts.Admin;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        public async Task<IActionResult> All()
        {
            return View();
        }
    }
}
