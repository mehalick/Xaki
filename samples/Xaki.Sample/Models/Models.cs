using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Xaki.Sample.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Planet> Planets { get; set; }
    }

    public class Planet : ILocalizable
    {
        public int PlanetId { get; set; }

        [Required, Localized]
        public string Name { get; set; }
    }
}
