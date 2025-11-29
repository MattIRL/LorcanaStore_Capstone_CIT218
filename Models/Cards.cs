using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LorcanaCardCollector.Models
{

    public enum GemColor
    {
        // Single colors
        Amber,
        Amethyst,
        Emerald,
        Ruby,
        Sapphire,
        Steel,

        // Two-color combinations
        [Display(Name ="Amber & Amethyst")]
        AmberAmethyst,
        [Display(Name = "Amber & Emerald")]
        AmberEmerald,
        [Display(Name = "Amber & Ruby")]
        AmberRuby,
        [Display(Name = "Amber & Sapphire")]
        AmberSapphire,
        [Display(Name = "Amber & Steel")]
        AmberSteel,
        [Display(Name = "Amethyst & Emerald")]
        AmethystEmerald,
        [Display(Name = "Amethyst & Ruby")]
        AmethystRuby,
        [Display(Name = "Amethyst & Sapphire")]
        AmethystSapphire,
        [Display(Name = "Amethyst & Steel")]
        AmethystSteel,
        [Display(Name = "Emerald & Ruby")]
        EmeraldRuby,
        [Display(Name = "Emerald & Sapphire")]
        EmeraldSapphire,
        [Display(Name = "Emerald & Steel")]
        EmeraldSteel,
        [Display(Name = "Ruby & Sapphire")]
        RubySapphire,
        [Display(Name = "Ruby & Steel")]
        RubySteel,
        [Display(Name = "Sapphire & Steel")]
        SapphireSteel, 
    }
    public class Cards
    {
        public ICollection<DeckCard> DeckCards { get; set; } = new List<DeckCard>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CardId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Card Name")]
        [StringLength(100, ErrorMessage = "Please enter a card name using 100 characters or less.")]
        public string CardName { get; set; } = string.Empty;

        [Display(Name = "Franchise")]
        [StringLength(100)]
        public string? Franchise { get; set; }

        [Display(Name = "Card Image URL")]
        [StringLength(500)]
        [Column("Image_URL", TypeName = "VARCHAR(500)")]
        public string? Image_URL { get; set; }

        [Required]
        [Display(Name = "Color Identity")]
        public GemColor GemColor { get; set; }

        [Display(Name ="Ink Cost")]
        public int? Ink { get; set; }

        [Display(Name = "Willpower (Health)")]
        public int? Willpower { get; set; }

        [Display(Name = "Strength (Attack)")]
        public int? Strength { get; set; }

        [Required]
        [Display(Name = "Set Name")]
        [StringLength(100, ErrorMessage = "Please enter a set name using 100 characters or less.")]
        [RegularExpression(@"^[\p{L}0-9 ',\-&\(\)]+$",
    ErrorMessage = "Set Name contains invalid characters. Only letters, numbers, spaces, and common punctuation (',-&()) are allowed.")]
        public string SetName { get; set; } = string.Empty;
    }
}
