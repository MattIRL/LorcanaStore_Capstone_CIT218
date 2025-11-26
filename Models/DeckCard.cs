using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LorcanaCardCollector.Models
{
    public class DeckCard
    {
        // FK from Card Table
        [Required]
        [ForeignKey(nameof(Card))]
        public string CardId { get; set; }
        public Cards Card { get; set; }

        // FK from Deck Table
        [Required]
        [ForeignKey(nameof(Deck))]
        public int DeckId { get; set; }
        public Deck Deck { get; set; }

        public int QuantityInDeck { get; set; }

    }
}
