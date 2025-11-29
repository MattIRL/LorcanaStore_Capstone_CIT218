namespace LorcanaCardCollector.Models
{
    public class EditDeckViewModel
    {
        public int DeckId { get; set; }
        public string DeckName { get; set; }
        public List<CardCheckboxItem> Cards { get; set; } = new();
    }

    public class CardCheckboxItem
    {
        public string CardId { get; set; }
        public string CardName { get; set; }
        public bool IsSelected { get; set; }
    }
}
