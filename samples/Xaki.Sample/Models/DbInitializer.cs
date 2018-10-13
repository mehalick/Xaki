using System.Collections.Generic;
using System.Linq;

namespace Xaki.Sample.Models
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            if (context.Planets.Any())
            {
                return;
            }

            // https://en.wiktionary.org/wiki/Appendix:Planets
            // https://solarsystem.nasa.gov/planets/overview/
            // https://solarsystem.nasa.gov/moons/overview/

            var planets = new HashSet<Planet>
            {
                new Planet
                {
                    Name = "{'en':'Mercury','zh':'水星','ar':'عطارد','es':'Mercurio','hi':'बुध','pt':'Mercúrio','ru':'Мерку́рий','ja':'水星','de':'Merkur','el':'Ερμής'}",
                    Description = "{'en':'The sun-scorched innermost planet is an intriguing world of extremes.'}",
                    ImageUrl = "https://solarsystem.nasa.gov/system/stellar_items/list_view_images/2_mercury_480x320_new.jpg",
                    Moons = new HashSet<Moon>()
                },
                new Planet
                {
                    Name = "{'en':'Venus','zh':'金星','ar':'الزهرة','es':'Venus','hi':'शुक्र','pt':'Vénus','ru':'Вене́ра','ja':'金星','de':'Venus','el':'Αφροδίτη'}",
                    Description = "{'en':'Venus is the second planet from the sun and our closest planetary neighbor.'}",
                    ImageUrl = "https://solarsystem.nasa.gov/system/stellar_items/list_view_images/3_480x320_venus.png",
                    Moons = new HashSet<Moon>()
                },
                new Planet
                {
                    Name = "{'en':'Earth','zh':'地球','ar':'الأرض','es':'Tierra','hi':'पृथ्वी','pt':'Terra','ru':'Земля́','ja':'地球','de':'Erde','el':'Γη'}",
                    Description = "{'en':'Earth is the third planet from the sun and the fifth largest in the solar system.'}",
                    ImageUrl = "https://solarsystem.nasa.gov/system/stellar_items/list_view_images/4_earth_480x320.jpg",
                    Moons = new HashSet<Moon>
                    {
                        new Moon { Name = "{'en':'Moon'}" }
                    }
                },
                new Planet
                {
                    Name = "{'en':'Mars','zh':'火星','ar':'المريخ','es':'Marte','hi':'मंगल','pt':'Marte','ru':'Марс','ja':'火星','de':'Mars','el':'Άρης'}",
                    Description = "{'en':'Mars may have once been a wet world like Earth. Where did the water go?'}",
                    ImageUrl = "https://solarsystem.nasa.gov/system/stellar_items/list_view_images/6_mars_480x320.jpg",
                    Moons = new HashSet<Moon>
                    {
                        new Moon { Name = "{'en':'Deimos'}" },
                        new Moon { Name = "{'en':'Phobos'}" }
                    }
                },
                new Planet
                {
                    Name = "{'en':'Jupiter','zh':'木星','ar':'المشتري','es':'Júpiter','hi':'बृहस्पति','pt':'Júpiter','ru':'Юпи́тер','ja':'木星','de':'Jupiter','el':'Δίας'}",
                    Description = "{'en':'Jupiter is the fifth planet from our sun and the largest planet in the solar system.'}",
                    ImageUrl = "https://solarsystem.nasa.gov/system/stellar_items/list_view_images/9_jupiter_480x320_new.jpg",
                    Moons = new HashSet<Moon>
                    {
                        new Moon { Name = "{'en':'Adrastea'}" },
                        new Moon { Name = "{'en':'Aitne'}" },
                        new Moon { Name = "{'en':'Amalthea'}" },
                        new Moon { Name = "{'en':'Ananke'}" },
                        new Moon { Name = "{'en':'Aoede'}" },
                        new Moon { Name = "{'en':'Arche'}" },
                        new Moon { Name = "{'en':'Autonoe'}" },
                        new Moon { Name = "{'en':'Callirrhoe'}" },
                        new Moon { Name = "{'en':'Callisto'}" },
                        new Moon { Name = "{'en':'Carme'}" },
                        new Moon { Name = "{'en':'Carpo'}" },
                        new Moon { Name = "{'en':'Chaldene'}" },
                        new Moon { Name = "{'en':'Cyllene'}" },
                        new Moon { Name = "{'en':'Dia'}" },
                        new Moon { Name = "{'en':'Elara'}" },
                        new Moon { Name = "{'en':'Erinome'}" },
                        new Moon { Name = "{'en':'Euanthe'}" },
                        new Moon { Name = "{'en':'Eukelade'}" },
                        new Moon { Name = "{'en':'Euporie'}" },
                        new Moon { Name = "{'en':'Europa'}" },
                        new Moon { Name = "{'en':'Eurydome'}" },
                        new Moon { Name = "{'en':'Ganymede'}" },
                        new Moon { Name = "{'en':'Harpalyke'}" },
                        new Moon { Name = "{'en':'Hegemone'}" },
                        new Moon { Name = "{'en':'Helike'}" },
                        new Moon { Name = "{'en':'Hermippe'}" },
                        new Moon { Name = "{'en':'Herse'}" },
                        new Moon { Name = "{'en':'Himalia'}" },
                        new Moon { Name = "{'en':'Io'}" },
                        new Moon { Name = "{'en':'Iocaste'}" },
                        new Moon { Name = "{'en':'Isonoe'}" },
                        new Moon { Name = "{'en':'Jupiter LI'}" },
                        new Moon { Name = "{'en':'Jupiter LII'}" },
                        new Moon { Name = "{'en':'Kale'}" },
                        new Moon { Name = "{'en':'Kallichore'}" },
                        new Moon { Name = "{'en':'Kalyke'}" },
                        new Moon { Name = "{'en':'Kore'}" },
                        new Moon { Name = "{'en':'Leda'}" },
                        new Moon { Name = "{'en':'Lysithea'}" },
                        new Moon { Name = "{'en':'Megaclite'}" },
                        new Moon { Name = "{'en':'Metis'}" },
                        new Moon { Name = "{'en':'Mneme'}" },
                        new Moon { Name = "{'en':'Orthosie'}" },
                        new Moon { Name = "{'en':'Pasiphae'}" },
                        new Moon { Name = "{'en':'Pasithee'}" },
                        new Moon { Name = "{'en':'Praxidike'}" },
                        new Moon { Name = "{'en':'S/2003 J10'}" },
                        new Moon { Name = "{'en':'S/2003 J12'}" },
                        new Moon { Name = "{'en':'S/2003 J15'}" },
                        new Moon { Name = "{'en':'S/2003 J16'}" },
                        new Moon { Name = "{'en':'S/2003 J18'}" },
                        new Moon { Name = "{'en':'S/2003 J19'}" },
                        new Moon { Name = "{'en':'S/2003 J2'}" },
                        new Moon { Name = "{'en':'S/2003 J23'}" },
                        new Moon { Name = "{'en':'S/2003 J3'}" },
                        new Moon { Name = "{'en':'S/2003 J4'}" },
                        new Moon { Name = "{'en':'S/2003 J5'}" },
                        new Moon { Name = "{'en':'S/2003 J9'}" },
                        new Moon { Name = "{'en':'S/2011 J1'}" },
                        new Moon { Name = "{'en':'S/2011 J2'}" },
                        new Moon { Name = "{'en':'S/2016 J1'}" },
                        new Moon { Name = "{'en':'S/2016 J2 (Valetudo)'}" },
                        new Moon { Name = "{'en':'S/2017 J1'}" },
                        new Moon { Name = "{'en':'S/2017 J2'}" },
                        new Moon { Name = "{'en':'S/2017 J3'}" },
                        new Moon { Name = "{'en':'S/2017 J4'}" },
                        new Moon { Name = "{'en':'S/2017 J5'}" },
                        new Moon { Name = "{'en':'S/2017 J6'}" },
                        new Moon { Name = "{'en':'S/2017 J7'}" },
                        new Moon { Name = "{'en':'S/2017 J8'}" },
                        new Moon { Name = "{'en':'S/2017 J9'}" },
                        new Moon { Name = "{'en':'S/2018 J1'}" },
                        new Moon { Name = "{'en':'Sinope'}" },
                        new Moon { Name = "{'en':'Sponde'}" },
                        new Moon { Name = "{'en':'Taygete'}" },
                        new Moon { Name = "{'en':'Thebe'}" },
                        new Moon { Name = "{'en':'Thelxinoe'}" },
                        new Moon { Name = "{'en':'Themisto'}" },
                        new Moon { Name = "{'en':'Thyone'}" }
                    }
                },
                new Planet
                {
                    Name = "{'en':'Saturn','zh':'土星','ar':'زحل','es':'Saturno','hi':'शनि','pt':'Saturno','ru':'Сату́рн','ja':'土星','de':'Saturn','el':'Κρόνος'}",
                    Description = "{'en':'The second largest planet in our solar system, adorned with thousands of beautiful ringlets, Saturn is unique among the planets.'}",
                    ImageUrl = "https://solarsystem.nasa.gov/system/stellar_items/list_view_images/38_saturn_480x320.jpg",
                    Moons = new HashSet<Moon>
                    {
                        new Moon { Name = "{'en':'Aegaeon'}" },
                        new Moon { Name = "{'en':'Aegir'}" },
                        new Moon { Name = "{'en':'Albiorix'}" },
                        new Moon { Name = "{'en':'Anthe'}" },
                        new Moon { Name = "{'en':'Atlas'}" },
                        new Moon { Name = "{'en':'Bebhionn'}" },
                        new Moon { Name = "{'en':'Bergelmir'}" },
                        new Moon { Name = "{'en':'Bestla'}" },
                        new Moon { Name = "{'en':'Calypso'}" },
                        new Moon { Name = "{'en':'Daphnis'}" },
                        new Moon { Name = "{'en':'Dione'}" },
                        new Moon { Name = "{'en':'Enceladus'}" },
                        new Moon { Name = "{'en':'Epimetheus'}" },
                        new Moon { Name = "{'en':'Erriapus'}" },
                        new Moon { Name = "{'en':'Farbauti'}" },
                        new Moon { Name = "{'en':'Fenrir'}" },
                        new Moon { Name = "{'en':'Fornjot'}" },
                        new Moon { Name = "{'en':'Greip'}" },
                        new Moon { Name = "{'en':'Hati'}" },
                        new Moon { Name = "{'en':'Helene'}" },
                        new Moon { Name = "{'en':'Hyperion'}" },
                        new Moon { Name = "{'en':'Hyrrokkin'}" },
                        new Moon { Name = "{'en':'Iapetus'}" },
                        new Moon { Name = "{'en':'Ijiraq'}" },
                        new Moon { Name = "{'en':'Janus'}" },
                        new Moon { Name = "{'en':'Jarnsaxa'}" },
                        new Moon { Name = "{'en':'Kari'}" },
                        new Moon { Name = "{'en':'Kiviuq'}" },
                        new Moon { Name = "{'en':'Loge'}" },
                        new Moon { Name = "{'en':'Methone'}" },
                        new Moon { Name = "{'en':'Mimas'}" },
                        new Moon { Name = "{'en':'Mundilfari'}" },
                        new Moon { Name = "{'en':'Narvi'}" },
                        new Moon { Name = "{'en':'Paaliaq'}" },
                        new Moon { Name = "{'en':'Pallene'}" },
                        new Moon { Name = "{'en':'Pan'}" },
                        new Moon { Name = "{'en':'Pandora'}" },
                        new Moon { Name = "{'en':'Phoebe'}" },
                        new Moon { Name = "{'en':'Polydeuces'}" },
                        new Moon { Name = "{'en':'Prometheus'}" },
                        new Moon { Name = "{'en':'Rhea'}" },
                        new Moon { Name = "{'en':'S/2004 S12'}" },
                        new Moon { Name = "{'en':'S/2004 S13'}" },
                        new Moon { Name = "{'en':'S/2004 S17'}" },
                        new Moon { Name = "{'en':'S/2004 S7'}" },
                        new Moon { Name = "{'en':'S/2006 S1'}" },
                        new Moon { Name = "{'en':'S/2006 S3'}" },
                        new Moon { Name = "{'en':'S/2007 S2'}" },
                        new Moon { Name = "{'en':'S/2007 S3'}" },
                        new Moon { Name = "{'en':'Siarnaq'}" },
                        new Moon { Name = "{'en':'Skathi'}" },
                        new Moon { Name = "{'en':'Skoll'}" },
                        new Moon { Name = "{'en':'Surtur'}" },
                        new Moon { Name = "{'en':'Suttungr'}" },
                        new Moon { Name = "{'en':'Tarqeq'}" },
                        new Moon { Name = "{'en':'Tarvos'}" },
                        new Moon { Name = "{'en':'Telesto'}" },
                        new Moon { Name = "{'en':'Tethys'}" },
                        new Moon { Name = "{'en':'Thrymr'}" },
                        new Moon { Name = "{'en':'Titan'}" },
                        new Moon { Name = "{'en':'Ymir'}" }
                    }
                },
                new Planet
                {
                    Name = "{'en':'Uranus','zh':'天王星','ar':'أورانوس','es':'Urano','hi':'अरुण','pt':'Urano','ru':'Ура́н','ja':'天王星','de':'Uranus','el':'Ουρανός'}",
                    Description = "{'en':'The seventh planet from the sun with the third largest diameter in our solar system, Uranus is very cold and windy.'}",
                    ImageUrl = "https://solarsystem.nasa.gov/system/stellar_items/list_view_images/69_uranus_480x320.jpg",
                    Moons = new HashSet<Moon>
                    {
                        new Moon { Name = "{'en':'Ariel'}" },
                        new Moon { Name = "{'en':'Belinda'}" },
                        new Moon { Name = "{'en':'Bianca'}" },
                        new Moon { Name = "{'en':'Caliban'}" },
                        new Moon { Name = "{'en':'Cordelia'}" },
                        new Moon { Name = "{'en':'Cressida'}" },
                        new Moon { Name = "{'en':'Cupid'}" },
                        new Moon { Name = "{'en':'Desdemona'}" },
                        new Moon { Name = "{'en':'Ferdinand'}" },
                        new Moon { Name = "{'en':'Francisco'}" },
                        new Moon { Name = "{'en':'Juliet'}" },
                        new Moon { Name = "{'en':'Mab'}" },
                        new Moon { Name = "{'en':'Margaret'}" },
                        new Moon { Name = "{'en':'Miranda'}" },
                        new Moon { Name = "{'en':'Oberon'}" },
                        new Moon { Name = "{'en':'Ophelia'}" },
                        new Moon { Name = "{'en':'Perdita'}" },
                        new Moon { Name = "{'en':'Portia'}" },
                        new Moon { Name = "{'en':'Prospero'}" },
                        new Moon { Name = "{'en':'Puck'}" },
                        new Moon { Name = "{'en':'Rosalind'}" },
                        new Moon { Name = "{'en':'Setebos'}" },
                        new Moon { Name = "{'en':'Stephano'}" },
                        new Moon { Name = "{'en':'Sycorax'}" },
                        new Moon { Name = "{'en':'Titania'}" },
                        new Moon { Name = "{'en':'Trinculo'}" },
                        new Moon { Name = "{'en':'Umbriel'}" }
                    }
                },
                new Planet
                {
                    Name = "{'en':'Neptune','zh':'海王星','ar':'نبتون','es':'Neptuno','hi':'वरुण','pt':'Neptuno','ru':'Непту́н','ja':'海王星','de':'Neptun','el':'Ποσειδώνας'}",
                    Description = "{'en':'Dark, cold and whipped by supersonic winds, Neptune is the last of the hydrogen and helium gas giants in our solar system.'}",
                    ImageUrl = "https://solarsystem.nasa.gov/system/stellar_items/list_view_images/90_neptune_480x320.jpg",
                    Moons = new HashSet<Moon>
                    {
                        new Moon { Name = "{'en':'Despina'}" },
                        new Moon { Name = "{'en':'Galatea'}" },
                        new Moon { Name = "{'en':'Halimede'}" },
                        new Moon { Name = "{'en':'Laomedeia'}" },
                        new Moon { Name = "{'en':'Larissa'}" },
                        new Moon { Name = "{'en':'Naiad'}" },
                        new Moon { Name = "{'en':'Nereid'}" },
                        new Moon { Name = "{'en':'Neso'}" },
                        new Moon { Name = "{'en':'Proteus'}" },
                        new Moon { Name = "{'en':'Psamathe'}" },
                        new Moon { Name = "{'en':'S/2004 N1'}" },
                        new Moon { Name = "{'en':'Sao'}" },
                        new Moon { Name = "{'en':'Thalassa'}" },
                        new Moon { Name = "{'en':'Triton'}" }
                    }
                }
                //new Planet
                //{
                //    Name = "{'en':'Pluto','zh':'冥王星','ar':'بلوتو','es':'Plutón','hi':'प्लूटो','pt':'Plutão','ru':'Плуто́н','ja':'冥王星','de':'Pluto','el':'Πλούτων'}",
                //    Description = "{'en':'Pluto is the best known world of the vast, intriguing Kuiper Belt.'}",
                //    ImageUrl = "https://solarsystem.nasa.gov/planets/dwarf-planets/pluto/overview",
                //    Moons = new HashSet<Moon>
                //    {
                //        new Moon { Name = "{'en':'Charon'}" },
                //        new Moon { Name = "{'en':'Hydra'}" },
                //        new Moon { Name = "{'en':'Kerberos'}" },
                //        new Moon { Name = "{'en':'Nix'}" },
                //        new Moon { Name = "{'en':'Styx'}" }
                //    }
                //}
            };

            foreach (var planet in planets)
            {
                context.Add(planet);
            }

            context.SaveChanges();
        }
    }
}