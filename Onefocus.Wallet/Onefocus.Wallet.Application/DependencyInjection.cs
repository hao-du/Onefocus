using Microsoft.Extensions.DependencyInjection;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Services;

namespace Onefocus.Wallet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IBankService, BankService>();

        return services;
    }
}
