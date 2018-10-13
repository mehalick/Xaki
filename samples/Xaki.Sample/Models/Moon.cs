using System.ComponentModel.DataAnnotations;

namespace Xaki.Sample.Models
{
    public class Moon : ILocalizable
    {
        public int MoonId { get; set; }

        public int PlanetId { get; set; }

        [Required, Localized]
        public string Name { get; set; }

        public virtual Planet Planet { get; set; }
    }
}