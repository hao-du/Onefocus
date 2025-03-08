using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Configurations;
using Onefocus.Common.Constants;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Membership.Infrastructure.Databases.DbContexts;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.ServiceBus;

namespace Onefocus.Membership.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MembershipDbContext>(option => option.UseNpgsql(configuration.GetConnectionString("MembershipDatabase")));

        IMessageBrokerSettings messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<MembershipDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider(Commons.TokenProviderName, typeof(DataProtectorTokenProvider<User>));

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddMassTransit(busConfigure =>
        {
            busConfigure.SetKebabCaseEndpointNameFormatter();

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

        services.AddScoped<IUserSyncedPublisher, UserSyncedPublisher>();

        return services;
    }
}
