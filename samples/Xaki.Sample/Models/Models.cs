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
                new Planet { Name = "{'en':'Venus','zh':'金星','ar':'الزهرة','es':'Venus','hi':'शुक्र','pt':'Vénus','ru':'Вене́ра','ja':'金星','de':'Venus','el':'Αφροδίτη'}" },
                new Planet { Name = "{'en':'Earth','zh':'地球','ar':'الأرض','es':'Tierra','hi':'पृथ्वी','pt':'Terra','ru':'Земля́','ja':'地球','de':'Erde','el':'Γη'}" },
                new Planet { Name = "{'en':'Mars','zh':'火星','ar':'المريخ','es':'Marte','hi':'मंगल','pt':'Marte','ru':'Марс','ja':'火星','de':'Mars','el':'Άρης'}" },
                new Planet { Name = "{'en':'Jupiter','zh':'木星','ar':'المشتري','es':'Júpiter','hi':'बृहस्पति','pt':'Júpiter','ru':'Юпи́тер','ja':'木星','de':'Jupiter','el':'Δίας'}" },
                new Planet { Name = "{'en':'Saturn','zh':'土星','ar':'زحل','es':'Saturno','hi':'शनि','pt':'Saturno','ru':'Сату́рн','ja':'土星','de':'Saturn','el':'Κρόνος'}" },
                new Planet { Name = "{'en':'Uranus','zh':'天王星','ar':'أورانوس','es':'Urano','hi':'अरुण','pt':'Urano','ru':'Ура́н','ja':'天王星','de':'Uranus','el':'Ουρανός'}" },
                new Planet { Name = "{'en':'Neptune','zh':'海王星','ar':'نبتون','es':'Neptuno','hi':'वरुण','pt':'Neptuno','ru':'Непту́н','ja':'海王星','de':'Neptun','el':'Ποσειδώνας'}" }
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
