using Microsoft.AspNetCore.Mvc;
using NetBootcamp.Web.Models;
using NetBootcamp.Web.Services.Weather;
using System.Diagnostics;

namespace NetBootcamp.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, WeatherService weatherService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Weather = await weatherService.GetWeatherBetterVersionAsync("Istanbul");
            return View(default);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
