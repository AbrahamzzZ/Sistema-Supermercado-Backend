using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities.IA;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOllamaClient(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddHttpClient<OllamaClient>()
                .ConfigureHttpClient(client =>
                {
                    var baseUrl = config["OLLAMA_BASE_URL"] ?? "http://localhost:11434";
                    client.BaseAddress = new Uri(baseUrl);
                    client.Timeout = TimeSpan.FromMinutes(5);
                });

            return services;
        }
    }
}
