using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xaki.Web.Models;
using Xaki.Web.Services;

namespace Xaki.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PlanetService _planetService;

        public HomeController(DataContext dataContext, ILocalizationService localizationService)
        {
            _planetService = new PlanetService(dataContext, localizationService);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = await _planetService.GetPlanets("en");

            return View(model);
        }
    }
}
