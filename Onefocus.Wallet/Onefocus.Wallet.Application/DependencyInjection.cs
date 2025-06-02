using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Configurations;
using Onefocus.Wallet.Application.User.ServiceBus;

namespace Onefocus.Wallet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services
        , IConfiguration configuration
    )
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        MessageBrokerSettings messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;
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

        return services;
    }
}
