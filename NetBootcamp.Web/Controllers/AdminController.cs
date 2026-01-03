using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace NetBootcamp.Web.Controllers
{
    [Authorize(Roles = "editor")]
    public class AdminController(IDataProtectionProvider dataProtectionProvider) : Controller
    {
        public IActionResult Index()
        {
            var dataProtector = dataProtectionProvider.CreateProtector("thisIsMyProctectorKey");
            ViewBag.BgColor = dataProtector.Unprotect(HttpContext.Request.Cookies["bg-color"]!);
            return View();
        }
    }
}
