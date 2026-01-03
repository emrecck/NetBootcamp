using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.Web.Models;
using NetBootcamp.Web.Services.User;
using NetBootcamp.Web.Services.User.Signin;

namespace NetBootcamp.Web.Controllers
{
    public class AuthController(UserService userService, ILogger<AuthController> logger) : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SigninViewModel model)
        {
            var response = await userService.SignIn(new SigninRequestDto(model.Email, model.Password, model.RememberMe));
            if (!response.IsSuccess)
            {
                response.FailMessages!.ForEach(error => ModelState.AddModelError(string.Empty, error));
                ViewBag.Error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(model);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

    }
}
