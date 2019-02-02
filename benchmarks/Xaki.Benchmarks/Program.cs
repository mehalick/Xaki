using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using Xaki.Benchmarks.Models;

namespace Xaki.Benchmarks
{
    internal class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }

    [ClrJob, MonoJob, CoreJob]
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private readonly Consumer _consumer = new Consumer();
        private readonly IObjectLocalizer _localizer;
        private readonly IEnumerable<Planet> _planets;

        public Benchmarks()
        {
            _localizer = new ObjectLocalizer
            {
                RequiredLanguages = new HashSet<string> { "en", "zh", "ar", "es", "hi" },
                OptionalLanguages = new HashSet<string> { "pt", "ru", "ja", "de", "el" }
            };

            _planets = CreatePlanets(1000);
        }

        [Benchmark]
        public void Shallow()
        {
            _localizer.Localize(_planets, "en", LocalizationDepth.Shallow).Consume(_consumer);
        }

        [Benchmark]
        public void OneLevel()
        {
            _localizer.Localize(_planets, "en", LocalizationDepth.OneLevel).Consume(_consumer);
        }

        [Benchmark]
        public void Deep()
        {
            _localizer.Localize(_planets, "en", LocalizationDepth.Deep).Consume(_consumer);
        }

        /// <summary>
        /// Creates X number of planets with X number of moons, all localized properties include all supported languages.
        /// </summary>
        private IEnumerable<Planet> CreatePlanets(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var name = _localizer.SupportedLanguages.ToDictionary(k => k, k => $"Planet {i}");
                var description = _localizer.SupportedLanguages.ToDictionary(k => k, k => $"Description {i}");

                var planet = new Planet
                {
                    PlanetId = i,
                    Name = _localizer.Serialize(name),
                    Description = _localizer.Serialize(description),
                    ImageUrl = "https://...",
                    Moons = new HashSet<Moon>()
                };

                for (var j = 0; j < count; j++)
                {
                    var moonName = _localizer.SupportedLanguages.ToDictionary(k => k, k => $"Moon {j}");

                    planet.Moons.Add(new Moon
                    {
                        MoonId = j,
                        Name = _localizer.Serialize(moonName),
                        Planet = planet,
                        PlanetId = planet.PlanetId
                    });
                }

                yield return planet;
            }
        }
    }
}
