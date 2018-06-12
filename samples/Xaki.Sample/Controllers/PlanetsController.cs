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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var planets = (await _context.Planets.ToListAsync()).Localize(_localizer);

            return View(planets);
        }

        [HttpGet("{planetId:int}")]
        public async Task<IActionResult> Edit(int planetId)
        {
            var planet = await _context.Planets.SingleOrDefaultAsync(i => i.PlanetId == planetId);
            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }

        [HttpPost("{planetId:int}")]
        public async Task<IActionResult> EditPost(int planetId)
        {
            var planet = await _context.Planets.SingleOrDefaultAsync(i => i.PlanetId == planetId);
            if (planet == null)
            {
                return NotFound();
            }

            await TryUpdateModelAsync(planet);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
