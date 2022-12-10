using CarDealership.Core.Constants;
using CarDealership.Core.Contracts.Admin;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        public async Task<IActionResult> All()
        {
            var model = await userService.All();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Clear(string userId)
        {
            bool result = await userService.Clear(userId);

            if (result)
            {
                TempData[MessageConstant.SuccessMessage] = "User is deleted";
            }
            else
            {
                TempData[MessageConstant.ErrorMessage] = "Something is wrong";
            }

            return RedirectToAction(nameof(All));
        }
    }
}
