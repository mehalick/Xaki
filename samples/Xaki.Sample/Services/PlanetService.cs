using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xaki.Sample.Models;

namespace Xaki.Sample.Services
{
    public class PlanetService
    {
        private readonly DataContext _dataContext;
        private readonly ILocalizationService _localizationService;

        public PlanetService(DataContext dataContext, ILocalizationService localizationService)
        {
            _dataContext = dataContext;
            _localizationService = localizationService;
        }

        public async Task<IReadOnlyCollection<Planet>> GetPlanets()
        {
            var planets = await _dataContext.Planets.ToListAsync();

            return _localizationService.Localize<Planet>(planets).ToList();
        }
    }
}
