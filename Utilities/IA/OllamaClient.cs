using System.Net.Http.Json;

namespace Utilities.IA
{
    public class OllamaClient
    {
        private readonly HttpClient _http;

        public OllamaClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GenerateAsync(string prompt, string model = "phi3.5")
        {
            var body = new
            {
                model,
                prompt,
                stream = false,
                temperature = 0.3,
                num_predict = 50,
                top_p = 0.9
            };

            try
            {
                var response = await _http.PostAsJsonAsync("/api/generate", body);

                if (!response.IsSuccessStatusCode)
                    return $"Error en IA: {response.StatusCode}";

                var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
                return result?.Response ?? "Sin respuesta generada.";
            }
            catch (Exception ex)
            {
                return $"Error conectando a Ollama: {ex.Message}";
            }
        }
    }

    public class OllamaResponse
    {
        public string? Response { get; set; }
    }
}
