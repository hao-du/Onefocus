using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Configurations;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure.Http;
using Onefocus.Search.Application.Interfaces.Repositories;
using Onefocus.Search.Application.Interfaces.Services;
using Onefocus.Search.Application.Interfaces.UnitOfWork;
using Onefocus.Search.Infrastructure.Databases.DbContexts;
using Onefocus.Search.Infrastructure.Repositories;
using Onefocus.Search.Infrastructure.ServiceBus;
using Onefocus.Search.Infrastructure.Services;
using OpenSearch.Client;
using System.Text;

namespace Onefocus.Search.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ILogger logger,
        IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddMessage(configuration)
            .AddSearch(logger, configuration)
            .AddClient(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("SearchDatabase");

        services.AddDbContext<SearchDbContext>(option =>
        {
            option.UseNpgsql(databaseConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });

        return services;
    }

    private static IServiceCollection AddMessage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;
        services.AddMassTransit(busConfigure =>
        {
            busConfigure.AddConsumer<SearchIndexConsumer>().Endpoint(configure =>
            {
                configure.InstanceId = $"-consumer-search-index-{messageBrokerSettings.InstanceId}";
            });

            busConfigure.AddConsumer<SearchSchemaConsumer>().Endpoint(configure =>
            {
                configure.InstanceId = $"-consumer-search-schema-{messageBrokerSettings.InstanceId}";
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

                configure.Message<ISearchSchemaMessage>(message =>
                {
                    message.SetEntityName(MessageQueueNames.SearchSchema);
                });

                configure.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    private static IServiceCollection AddSearch(
        this IServiceCollection services,
        ILogger logger,
        IConfiguration configuration)
    {
        var searchSettings = configuration.GetSection(ISearchSettings.SettingName).Get<SearchSettings>()!;
        services.AddSingleton<ISearchSettings>(searchSettings);

        var settings = new ConnectionSettings(new Uri(searchSettings.Url))
            .DisableDirectStreaming()
            .OnRequestCompleted(callDetails =>
            {
                if (callDetails.RequestBodyInBytes != null)
                {
                    logger.LogInformation("Request call details: httpMethod - {HttpMethod}, uri - {URI}, requestbody - {RequestBody}",
                        callDetails.HttpMethod, callDetails.Uri, Encoding.UTF8.GetString(callDetails.RequestBodyInBytes));
                }
            });

        services.AddSingleton<IOpenSearchClient>(new OpenSearchClient(settings));

        services.AddScoped<IEmbeddingService, EmbeddingService>();
        services.AddScoped<ISearchSchemaService, SearchSchemaService>();
        services.AddScoped<ISearchIndexService, SearchIndexService>();

        return services;
    }

    private static IServiceCollection AddClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var embeddingSettings = configuration.GetSection(IEmbeddingSettings.SettingName).Get<EmbeddingSettings>()!;

        services
            .AddHttpClientWrapper()
            .AddResilientHttpClients(
                HttpClientNames.Embedding,
                embeddingSettings.BaseAddress,
                (headers) =>
                {
                    headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                }
            );

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<ISearchIndexQueueRepository, SearchIndexQueueRepository>();

        return services;
    }
}
