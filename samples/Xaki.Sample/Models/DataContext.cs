using Microsoft.EntityFrameworkCore;

namespace Xaki.Sample.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Planet> Planets { get; set; }
        public DbSet<Moon> Moons { get; set; }
    }
}