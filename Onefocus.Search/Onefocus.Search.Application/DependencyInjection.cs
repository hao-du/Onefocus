using Microsoft.Extensions.DependencyInjection;
using Onefocus.Search.Application.BackgroundServices;
using Onefocus.Search.Application.Interfaces.Services;
using Onefocus.Search.Application.Services;

namespace Onefocus.Search.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<ISearchIndexManagementService, SearchIndexManagementService>();

        services.AddHostedService<SearchIndexHostedService>();

        return services;
    }
}
