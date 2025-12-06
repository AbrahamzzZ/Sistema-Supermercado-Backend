using System.Net.Http.Json;

namespace Utilities.IA
{
    public class OllamaClient
    {
        private readonly HttpClient _http;

        public OllamaClient(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:11434");
        }

        public async Task<string> GenerateAsync(string prompt, string model = "qwen3:8b")
        {
            var body = new
            {
                model,
                prompt,
                stream = false
            };

            var response = await _http.PostAsJsonAsync("/api/generate", body);

            if (!response.IsSuccessStatusCode)
                return $"Error en IA: {response.StatusCode}";

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

            return result?.Response ?? "Sin respuesta generada.";
        }
    }

    public class OllamaResponse
    {
        public string? Response { get; set; }
    }
}
