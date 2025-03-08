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
using Onefocus.Common.Security;
using Onefocus.Common.Configurations;
using Microsoft.Extensions.Options;

namespace Onefocus.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(option => option.UseNpgsql(configuration.GetConnectionString("IdentityDatabase")));

        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddApiEndpoints()
            .AddTokenProvider(Commons.TokenProviderName, typeof(DataProtectorTokenProvider<User>));

        IMessageBrokerSettings messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;

        services.AddMassTransit(busConfigure =>
        {
            busConfigure.AddConsumer<UserSyncedConsumer>().Endpoint(configure =>
            {
                configure.InstanceId = messageBrokerSettings.InstanceId;
            });

            busConfigure.UsingRabbitMq((context, configure) =>
            {
                configure.Host(new Uri(messageBrokerSettings.Host), host =>
                {
                    host.Username(messageBrokerSettings.UserName);
                    host.Password(messageBrokerSettings.Password);
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