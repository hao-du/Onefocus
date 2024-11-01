using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Identity.Infrastructure.Databases.DbContexts;
using Onefocus.Identity.Domain.Entities;
using Onefocus.Common.Constants;
using Onefocus.Identity.Infrastructure.Security;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using MassTransit;
using Onefocus.Identity.Infrastructure.ServiceBus;

namespace Onefocus.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(option =>
            option.UseNpgsql(configuration.GetConnectionString("IdentityDatabase")));

        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddApiEndpoints()
            .AddTokenProvider(Commons.TokenProviderName, typeof(DataProtectorTokenProvider<User>));

        services.AddMassTransit(busConfigure =>
        {
            busConfigure.AddConsumer<UserSyncedConsumer>().Endpoint(configure =>
            {
                configure.InstanceId = Guid.NewGuid().ToString();
            });

            busConfigure.UsingRabbitMq((context, configure) =>
            {
                configure.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });

                configure.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();

        return services;
    }
}