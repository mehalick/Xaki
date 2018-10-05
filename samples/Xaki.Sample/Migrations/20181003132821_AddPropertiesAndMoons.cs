using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xaki.Sample.Migrations
{
    public partial class AddPropertiesAndMoons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Planets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Planets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Moons",
                columns: table => new
                {
                    MoonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanetId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moons", x => x.MoonId);
                    table.ForeignKey(
                        name: "FK_Moons_Planets_PlanetId",
                        column: x => x.PlanetId,
                        principalTable: "Planets",
                        principalColumn: "PlanetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Moons_PlanetId",
                table: "Moons",
                column: "PlanetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moons");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Planets");
        }
    }
}
