using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace LorcanaCardCollector.Models

{
    public class LorcanaCardDto
    {
        public string? Image { get; set; }
        public string? Name { get; set; }
    }
    public class LorcanaCardCollectorModel
    {
        [Required(ErrorMessage = "<p class='text.danger'>Please enter some text to search.</p>")]
        public string? CardName { get; set; }

        public async Task<List<LorcanaCardDto>> FetchCardAsync()
        {
            if (string.IsNullOrWhiteSpace(CardName))
                return new List<LorcanaCardDto>();


            string url = $"https://api.lorcana-api.com/cards/fetch?search=name~{CardName}";
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<LorcanaCardDto>();
            var json = await response.Content.ReadAsStringAsync();
            var cards = JsonSerializer.Deserialize<List<LorcanaCardDto>>(json);

            return cards ?? new List<LorcanaCardDto>();

        }
        /*
        // This will become a dynamic search return based on the https://lorcana-api.com/

        public string FetchCard()
        {
            string? cardName = CardName;
            string CardResults = "~/Images/rhino-motivational_speaker-large.png";
            return CardResults;
        }
        */
    }
}
// [DatabaseGenerated(DatabaseGeneratiedOption.None)]
