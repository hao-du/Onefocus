using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Configurations;
using Onefocus.Common.Constants;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Membership.Infrastructure.Databases.DbContexts;

namespace Onefocus.Membership.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MembershipDbContext>(option => option.UseNpgsql(configuration.GetConnectionString("MembershipDatabase")));
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<MembershipDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(Common.Constants.Common.TokenProviderName);

        var messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;
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

                configure.Message<ISyncUserMessage>(message =>
                {
                    message.SetEntityName(MessageQueueNames.SyncUser);
                });

                configure.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
