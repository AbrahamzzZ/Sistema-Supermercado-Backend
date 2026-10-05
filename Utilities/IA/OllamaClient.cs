using System.Net.Http.Json;

namespace Utilities.IA
{
    public class OllamaClient
    {
        private readonly HttpClient _http;
        private readonly OllamaOptions _options;

        public OllamaClient(HttpClient http, OllamaOptions options)
        {
            _http = http;
            _options = options;
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            var body = new
            {
                model = _options.Model,
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
