using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Onefocus.Common.Infrastructure.Http;

public static class ResilienceRegistration
{
    public static IServiceCollection AddHttpClientWrapper(
        this IServiceCollection services
    )
    {
        services.AddTransient<IdempotencyHandler>();
        services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

        return services;
    }

    public static IServiceCollection AddResilientHttpClients(
        this IServiceCollection services,
        string name,
        string baseAddress,
        Action<HttpRequestHeaders>? configureHeaders = null,
        int? maxRetryAttempts = null,
        TimeSpan? timeout = null
    )
    {
        services.AddHttpClient(name, client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            configureHeaders?.Invoke(client.DefaultRequestHeaders);
        })
            .AddHttpMessageHandler<IdempotencyHandler>()
            .AddStandardResilienceHandler(options =>
            {
                if (maxRetryAttempts.HasValue) options.Retry.MaxRetryAttempts = maxRetryAttempts.Value;
                if (timeout.HasValue) options.AttemptTimeout.Timeout = timeout.Value;
            });

        return services;
    }
}
