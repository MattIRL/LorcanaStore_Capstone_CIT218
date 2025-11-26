using Microsoft.EntityFrameworkCore;

namespace LorcanaCardCollector.Models
{
    public class CardsContext : DbContext
    {
        public CardsContext(DbContextOptions<CardsContext> options) : base(options)
        {
        }
        public DbSet<Cards> Cards { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<DeckCard> DeckCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeckCard>()
                .HasKey(dc => new { dc.CardId, dc.DeckId });
            // Relationship: DeckCard → Card (many-to-one)
            modelBuilder.Entity<DeckCard>()
                .HasOne(dc => dc.Card)
                .WithMany(c => c.DeckCards)
                .HasForeignKey(dc => dc.CardId);

            // Relationship: DeckCard → Deck (many-to-one)
            modelBuilder.Entity<DeckCard>()
                .HasOne(dc => dc.Deck)
                .WithMany(d => d.DeckCards)
                .HasForeignKey(dc => dc.DeckId);

            modelBuilder.Entity<Deck>().HasData(
                new Deck
                {
                    DeckId = 1,
                    DeckName = "Seed Deck",
                    DeckDescription = "Seed data for the deck",
                    AccessKey = "SeedDeck"
                }
                );
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory
                {
                    InventoryId = 1,
                    CardId = "ARI-003",
                    Quantity = 3,
                    Cost = 0.59m,
                    Price = 1.99m
                });



            modelBuilder.Entity<Cards>().HasData(

                new Cards
                {
                    ID = "ARI-003", /* API reports as "Unique_ID" */
                    CardName = "King Stefan - New Father", /* API reports as "Name" */
                    Franchise = "Sleeping Beauty",
                    Image_URL = "https://lorcana-api.com/images/king_stefan/new_father/king_stefan-new_father-large.png", /* API reports as "Image" */
                    Ink = 5, /* API reports as "Cost" */
                    GemColor = GemColor.Amber, /* API reports as "Color" */
                    SetName = "Archazia's Island", /* API reports as "Set_Name" */

                },

                new Cards
                {
                    ID = "ARI-001",
                    CardName = "Rhino - Motivational Speaker",
                    Franchise = "Bolt",
                    Image_URL = "https://lorcana-api.com/images/rhino/motivational_speaker/rhino-motivational_speaker-large.png",
                    Ink = 5,
                    GemColor = GemColor.AmberSteel,
                    SetName = "Archazia's Island"

                },

                new Cards
                {
                    ID = "ARI-002",
                    CardName = "Perdita - Playful Mother",
                    Franchise = "101 Dalmatians",
                    Image_URL = "https://lorcana-api.com/images/perdita/playful_mother/perdita-playful_mother-large.png",
                    GemColor = GemColor.AmberSapphire,
                    Ink = 4,
                    SetName = "Archazia's Island"

                }
            );
        }
    }
}