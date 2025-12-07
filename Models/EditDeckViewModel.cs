namespace LorcanaCardCollector.Models
{
    public class EditDeckViewModel
    {
        public int DeckId { get; set; }
        public string DeckName { get; set; }

        public string DeckDescription { get; set; }
        public List<DeckCardItem> Cards { get; set; } = new();
    }

    public class DeckCardItem
    {
        public string CardId { get; set; }
        public string CardName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
