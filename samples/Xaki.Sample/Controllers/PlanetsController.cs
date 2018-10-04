using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xaki.Sample.Models;

namespace Xaki.Sample.Controllers
{
    [Route("planets")]
    public class PlanetsController : Controller
    {
        private readonly DataContext _context;
        private readonly IObjectLocalizer _localizer;

        public PlanetsController(DataContext context, IObjectLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet("~/")]
        public IActionResult Redirect()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var planets = await _context.Planets.Include(i => i.Moons).ToListAsync();

            planets = _localizer.Localize<Planet>(planets).ToList();

            return View(planets);
        }

        [HttpGet("{planetId:int}")]
        public async Task<IActionResult> Details(int planetId)
        {
            var planet = await _context.Planets
                .Include(i => i.Moons)
                .SingleOrDefaultAsync(i => i.PlanetId == planetId);

            if (planet is null)
            {
                return NotFound();
            }

            planet = _localizer.Localize(planet, LocalizationDepth.OneLevel);

            return View(planet);
        }

        [HttpGet("{planetId:int}/edit")]
        public async Task<IActionResult> Edit(int planetId)
        {
            var planet = await _context.Planets.SingleOrDefaultAsync(i => i.PlanetId == planetId);

            if (planet is null)
            {
                return NotFound();
            }

            return View(planet);
        }

        [HttpPost("{planetId:int}/edit")]
        public async Task<IActionResult> Edit(Planet planet)
        {
            _context.Entry(planet).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{planetId:int}/moons/{moonId:int}/edit")]
        public async Task<IActionResult> EditMoon(int planetId, int moonId)
        {
            var moon = await _context.Moons.SingleOrDefaultAsync(i => i.PlanetId == planetId && i.MoonId == moonId);

            if (moon is null)
            {
                return NotFound();
            }

            return View(moon);
        }

        [HttpPost("{planetId:int}/moons/{moonId:int}/edit")]
        public async Task<IActionResult> EditMoon(Moon moon)
        {
            _context.Entry(moon).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { moon.PlanetId });
        }
    }
}
