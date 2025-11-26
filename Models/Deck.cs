using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace LorcanaCardCollector.Models
{
    public class Deck
    {
        public ICollection<DeckCard> DeckCards { get; set; } = new List<DeckCard>();

        [Key]
        public int DeckId { get; set; }

        [Required]
        [StringLength(100)]
        public string DeckName { get; set; } = string.Empty;
        [StringLength(500)]
        public string? DeckDescription { get; set; }

        [Required]
        [StringLength(20)]
        public string AccessKey {  get; set; }

    }
}
