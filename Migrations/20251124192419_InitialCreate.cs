using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LorcanaCardCollector.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Franchise = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Image_URL = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true),
                    GemColor = table.Column<int>(type: "int", nullable: false),
                    Ink = table.Column<int>(type: "int", nullable: true),
                    Willpower = table.Column<int>(type: "int", nullable: true),
                    Strength = table.Column<int>(type: "int", nullable: true),
                    SetName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "ID", "CardName", "Franchise", "GemColor", "Image_URL", "Ink", "SetName", "Strength", "Willpower" },
                values: new object[,]
                {
                    { "AIR-003", "King Stefan - New Father", "Sleeping Beauty", 0, "https://lorcana-api.com/images/king_stefan/new_father/king_stefan-new_father-large.png", 5, "Archazilia's Island", null, null },
                    { "ARI-001", "Rhino - Motivational Speaker", "Bolt", 10, "https://lorcana-api.com/images/rhino/motivational_speaker/rhino-motivational_speaker-large.png", 5, "Archazia's Island", null, null },
                    { "ARI-002", "Perdita - Playful Mother", "101 Dalmatians", 9, "https://lorcana-api.com/images/perdita/playful_mother/perdita-playful_mother-large.png", 4, "Archazia's Island", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
