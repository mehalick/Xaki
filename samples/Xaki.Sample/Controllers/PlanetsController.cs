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
        private readonly IObjectLocalizer _localizer;

        public PlanetsController(DataContext dataContext, IObjectLocalizer localizer)
        {
            _planetService = new PlanetService(dataContext);
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var planets = await _planetService.GetPlanets();

            planets = _localizer.Localize(planets);

            return View(planets);
        }

        [HttpGet("{planetId:int}")]
        public async Task<IActionResult> Edit(int planetId)
        {
            var planet = await _planetService.GetPlanetById(planetId);
            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }

        [HttpPost("{planetId:int}")]
        public async Task<IActionResult> Edit(int planetId, Planet model)
        {
            var planet = await _planetService.UpdatePlanet(planetId, model);
            if (planet == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
