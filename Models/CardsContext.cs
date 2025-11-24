using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace LorcanaCardCollector.Models
{
    public class CardsContext : DbContext
    {
        public CardsContext(DbContextOptions<CardsContext> options) : base(options)
        {
        }
        public DbSet<Cards> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cards>().HasData(

                new Cards
                {
                    ID = "AIR-003", /* API reports as "Unique_ID" */
                    CardName = "King Stefan - New Father", /* API reports as "Name" */
                    Franchise = "Sleeping Beauty",
                    Image_URL = "https://lorcana-api.com/images/king_stefan/new_father/king_stefan-new_father-large.png", /* API reports as "Image" */
                    Ink = 5, /* API reports as "Cost" */
                    GemColor = GemColor.Amber, /* API reports as "Color" */
                    SetName = "Archazilia's Island", /* API reports as "Set_Name" */

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