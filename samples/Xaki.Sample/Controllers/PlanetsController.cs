using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xaki.Sample.Models;
using Xaki.Sample.Services;

namespace Xaki.Sample.Controllers
{
    [Route("planets")]
    public class PlanetsController : Controller
    {
        private readonly PlanetService _planetService;

        public PlanetsController(DataContext dataContext, ILocalizationService localizationService)
        {
            _planetService = new PlanetService(dataContext, localizationService);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = await _planetService.GetPlanets();

            return View(model);
        }
    }
}
