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
                    CardId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    DeckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeckName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeckDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AccessKey = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.DeckId);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK_Inventories_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeckCards",
                columns: table => new
                {
                    CardId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    QuantityInDeck = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckCards", x => new { x.DeckId, x.CardId });
                    table.ForeignKey(
                        name: "FK_DeckCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckCards_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "CardId", "CardName", "Franchise", "GemColor", "Image_URL", "Ink", "SetName", "Strength", "Willpower" },
                values: new object[,]
                {
                    { "ARI-001", "Rhino - Motivational Speaker", "Bolt", 10, "https://lorcana-api.com/images/rhino/motivational_speaker/rhino-motivational_speaker-large.png", 5, "Archazia's Island", null, null },
                    { "ARI-002", "Perdita - Playful Mother", "101 Dalmatians", 9, "https://lorcana-api.com/images/perdita/playful_mother/perdita-playful_mother-large.png", 4, "Archazia's Island", null, null },
                    { "ARI-003", "King Stefan - New Father", "Sleeping Beauty", 0, "https://lorcana-api.com/images/king_stefan/new_father/king_stefan-new_father-large.png", 5, "Archazia's Island", null, null }
                });

            migrationBuilder.InsertData(
                table: "Decks",
                columns: new[] { "DeckId", "AccessKey", "DeckDescription", "DeckName" },
                values: new object[] { 1, "SeedDeck", "Seed data for the deck", "Seed Deck" });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "InventoryId", "CardId", "Cost", "Price", "Quantity" },
                values: new object[] { 1, "ARI-003", 0.59m, 1.99m, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_DeckCards_CardId",
                table: "DeckCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_CardId",
                table: "Inventories",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckCards");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
