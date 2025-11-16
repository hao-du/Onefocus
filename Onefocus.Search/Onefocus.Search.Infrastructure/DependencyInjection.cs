using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Configurations;
using Onefocus.Common.Constants;
using Onefocus.Search.Application.Interfaces.Services;
using Onefocus.Search.Infrastructure.ServiceBus;
using Onefocus.Search.Infrastructure.Services;
using OpenSearch.Client;

namespace Onefocus.Search.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;
        services.AddMassTransit(busConfigure =>
        {
            busConfigure.SetKebabCaseEndpointNameFormatter();

            busConfigure.AddConsumer<SearchIndexConsumer>().Endpoint(configure =>
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

                configure.Message<ISearchIndexMessage>(message =>
                {
                    message.SetEntityName(MessageQueueNames.SearchIndex);
                });

                configure.ConfigureEndpoints(context);
            });
        });

        var searchSettings = configuration.GetSection(ISearchSettings.SettingName).Get<SearchSettings>()!;
        services.AddSingleton<ISearchSettings>(searchSettings);

        var settings = new ConnectionSettings(new Uri(searchSettings.Url));
        var client = new OpenSearchClient(settings);
        services.AddSingleton<IOpenSearchClient>(client);

        services.AddScoped<IIndexingService, IndexingService>();

        return services;
    }
}
