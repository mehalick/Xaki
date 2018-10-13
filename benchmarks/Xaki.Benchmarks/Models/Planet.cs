using System.Collections.Generic;

namespace Xaki.Benchmarks.Models
{
    public class Planet : ILocalizable
    {
        public int PlanetId { get; set; }

        [Localized]
        public string Name { get; set; }

        [Localized]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<Moon> Moons { get; set; }
    }
}