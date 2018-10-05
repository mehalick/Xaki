namespace Xaki.Benchmarks.Models
{
    public class Moon : ILocalizable
    {
        public int MoonId { get; set; }

        public int PlanetId { get; set; }

        [Localized]
        public string Name { get; set; }

        public virtual Planet Planet { get; set; }
    }
}