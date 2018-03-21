using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;

namespace Xaki.Sample.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    PlanetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.PlanetId);
                });

            var planets = new[]
            {
                new { en = "Mercury", zh = "水星", ar = "عطارد", es = "Mercurio", hi = "बुध", pt = "Mercúrio", ru = "Мерку́рий", ja = "水星", de = "Merkur", el = "Ερμής" },
                new { en = "Venus", zh = "金星", ar = "الزهرة", es = "Venus", hi = "शुक्र", pt = "Vénus", ru = "Вене́ра", ja = "金星", de = "Venus", el = "Αφροδίτη" },
                new { en = "Earth", zh = "地球", ar = "الأرض", es = "Tierra", hi = "पृथ्वी", pt = "Terra", ru = "Земля́", ja = "地球", de = "Erde", el = "Γη" },
                new { en = "Mars", zh = "火星", ar = "المريخ", es = "Marte", hi = "मंगल", pt = "Marte", ru = "Марс", ja = "火星", de = "Mars", el = "Άρης" },
                new { en = "Jupiter", zh = "木星", ar = "المشتري", es = "Júpiter", hi = "बृहस्पति", pt = "Júpiter", ru = "Юпи́тер", ja = "木星", de = "Jupiter", el = "Δίας" },
                new { en = "Saturn", zh = "土星", ar = "زحل", es = "Saturno", hi = "शनि", pt = "Saturno", ru = "Сату́рн", ja = "土星", de = "Saturn", el = "Κρόνος" },
                new { en = "Uranus", zh = "天王星", ar = "أورانوس", es = "Urano", hi = "अरुण", pt = "Urano", ru = "Ура́н", ja = "天王星", de = "Uranus", el = "Ουρανός" },
                new { en = "Neptune", zh = "海王星", ar = "نبتون", es = "Neptuno", hi = "वरुण", pt = "Neptuno", ru = "Непту́н", ja = "海王星", de = "Neptun", el = "Ποσειδώνας" }
            };

            foreach (var planet in planets)
            {
                migrationBuilder.Sql($"insert into dbo.Planets (Name) values (N'{JsonConvert.SerializeObject(planet)}');");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Planets");
        }
    }
}
