using System.Globalization;
using System.Linq;
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
        private readonly ILocalizationService _localizationService;

        public PlanetsController(DataContext dataContext, ILocalizationService localizationService)
        {
            _planetService = new PlanetService(dataContext);
            _localizationService = localizationService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var planets = await _planetService.GetPlanets();

            CultureInfo.CurrentCulture = new CultureInfo("zh");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("zh");

            planets = _localizationService.Localize<Planet>(planets).ToList();

            return View(planets);
        }
    }
}
