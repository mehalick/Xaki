using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Xaki.Sample.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Planet> Planets { get; set; }
    }

    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            if (context.Planets.Any())
            {
                return;
            }

            var planets = new List<Planet>
            {
                new Planet { Name = "{'en':'Mercury','zh':'水星','ar':'عطارد','es':'Mercurio','hi':'बुध','pt':'Mercúrio','ru':'Мерку́рий','ja':'水星','de':'Merkur','el':'Ερμής'}" },
                // TODO: add renaming planets
            };

            foreach (var planet in planets)
            {
                context.Add(planet);
            }

            context.SaveChanges();
        }
    }

    public class Planet : ILocalizable
    {
        public int PlanetId { get; set; }

        [Required, Localized]
        public string Name { get; set; }
    }
}
