using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.Services.Search;
using Onefocus.Wallet.Application.Services;

namespace Onefocus.Wallet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<ISearchSchemaManagementService, SearchSchemaManagementService>();
        services.AddScoped<ICounterpartyService, CounterpartyService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IBankService, BankService>();

        return services;
    }

    public static async Task<WebApplication> UseSearch(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var schemaService = scope.ServiceProvider.GetRequiredService<ISearchSchemaManagementService>();
            await schemaService.InitializeSchemaAsync();
        }

        return app;
    }
}
