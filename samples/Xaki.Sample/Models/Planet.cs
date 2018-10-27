using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xaki.Sample.Models
{
    public class Planet : ILocalizable
    {
        public int PlanetId { get; set; }

        [Required, Localized]
        public string Name { get; set; }

        [Localized, DataType(DataType.Html)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<Moon> Moons { get; set; }
    }
}