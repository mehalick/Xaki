using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xaki.Sample.Models;

namespace Xaki.Sample.Services
{
    public class PlanetService
    {
        private readonly DataContext _dc;

        public PlanetService(DataContext dc)
        {
            _dc = dc;
        }

        public async Task<IEnumerable<Planet>> GetPlanets()
        {
            return await _dc.Planets.ToListAsync();
        }

        public async Task<Planet> GetPlanetById(int planetId)
        {
            return await _dc.Planets.SingleOrDefaultAsync(i => i.PlanetId == planetId);
        }

        public async Task<Planet> UpdatePlanet(int planetId, Planet model)
        {
            var planet = await _dc.Planets.SingleOrDefaultAsync(i => i.PlanetId == planetId);
            if (planet == null)
            {
                return null;
            }

            planet.Name = model.Name;

            await _dc.SaveChangesAsync();

            return planet;
        }
    }
}
