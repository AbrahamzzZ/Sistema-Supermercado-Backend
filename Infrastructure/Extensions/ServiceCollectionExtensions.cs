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
            // Sección "Ollama": appsettings en local, variables Ollama__* en Docker
            var options = config.GetSection(OllamaOptions.Seccion).Get<OllamaOptions>() ?? new OllamaOptions();
            services.AddSingleton(options);

            services.AddHttpClient<OllamaClient>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(options.BaseUrl);
                    client.Timeout = TimeSpan.FromMinutes(options.TimeoutMinutes);
                });

            return services;
        }
    }
}
