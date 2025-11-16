using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Configurations;
using Onefocus.Common.Constants;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Read;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Write;
using Onefocus.Home.Infrastructure.ServiceBus;

namespace Onefocus.Home.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var readDatabaseConnectionString = configuration.GetConnectionString("HomeReadDatabase");
        var writeDatabaseConnectionString = configuration.GetConnectionString("HomeWriteDatabase");

        services.AddDbContext<HomeReadDbContext>(option =>
        {
            option.UseNpgsql(readDatabaseConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
            option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddDbContext<HomeWriteDbContext>(option =>
            option.UseNpgsql(writeDatabaseConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
        );

        var messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;
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