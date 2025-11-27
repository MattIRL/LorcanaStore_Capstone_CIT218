using LorcanaCardCollector.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace LorcanaCardCollector.Services
{
    // Service for interacting with the Lorcana API
    public class LorcanaApiService
    {
        private readonly HttpClient _httpClient;

        public LorcanaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Search for cards by name.
        /// </summary>
        /// <param name="name">The search query.</param>
        /// <returns>List of CardApiModel objects matching the query.</returns>
        public async Task<List<CardApiModel>> SearchCardsAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<CardApiModel>();

            // Example endpoint: adjust to match your actual API route
            string endpoint = $"cards/fetch?search=name~{Uri.EscapeDataString(name)}";

            try
            {
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                    return new List<CardApiModel>();

                // Deserialize JSON response to List<CardApiModel>
                var cards = await response.Content.ReadFromJsonAsync<List<CardApiModel>>();
                return cards ?? new List<CardApiModel>();
            }
            catch
            {
                // In production, log the exception
                return new List<CardApiModel>();
            }
        }


    }
}
