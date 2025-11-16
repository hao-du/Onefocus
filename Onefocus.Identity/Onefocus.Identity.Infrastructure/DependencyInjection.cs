using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Configurations;
using Onefocus.Common.Constants;
using Onefocus.Identity.Domain.Entities;
using Onefocus.Identity.Infrastructure.Databases.DbContexts;
using Onefocus.Identity.Infrastructure.ServiceBus;

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
            .AddTokenProvider<DataProtectorTokenProvider<User>>(Common.Constants.Common.TokenProviderName);

        var messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;

        services.AddMassTransit(busConfigure =>
        {
            busConfigure.AddConsumer<SyncUserConsumer>().Endpoint(configure =>
            {
                configure.InstanceId = $"-consumer-identity-{messageBrokerSettings.InstanceId}";
            });

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