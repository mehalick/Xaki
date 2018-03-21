using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xaki.Sample.Models;

namespace Xaki.Sample.Services
{
    public class PlanetService
    {
        private readonly DataContext _dataContext;

        public PlanetService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IReadOnlyCollection<Planet>> GetPlanets()
        {
            return await _dataContext.Planets.ToListAsync();
        }
    }
}
