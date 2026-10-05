namespace Utilities.IA
{
    // Se llena desde la sección "Ollama" de la configuración (appsettings o variables Ollama__*)
    public class OllamaOptions
    {
        public const string Seccion = "Ollama";

        public string BaseUrl { get; set; } = "http://localhost:11434";
        public string Model { get; set; } = "phi3.5";
        public int TimeoutMinutes { get; set; } = 5;
    }
}
