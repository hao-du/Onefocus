using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Configurations;
using Onefocus.Common.Constants;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;
using Onefocus.Wallet.Infrastructure.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Repositories.Write;
using Onefocus.Wallet.Infrastructure.ServiceBus;
using Onefocus.Wallet.Infrastructure.ServiceBus.Search;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Write;

namespace Onefocus.Wallet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddMessage(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var readDatabaseConnectionString = configuration.GetConnectionString("WalletReadDatabase");
        var writeDatabaseConnectionString = configuration.GetConnectionString("WalletWriteDatabase");

        services.AddDbContext<WalletReadDbContext>(option =>
        {
            option.UseNpgsql(readDatabaseConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
            option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddDbContext<WalletWriteDbContext>(option =>
            option.UseNpgsql(writeDatabaseConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
        );

        return services;
    }

    private static IServiceCollection AddMessage(this IServiceCollection services, IConfiguration configuration)
    {
        var messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;
        services.AddMassTransit(busConfigure =>
        {
            busConfigure.AddConsumer<SyncUserConsumer>().Endpoint(configure =>
            {
                configure.InstanceId = $"-consumer-wallet-{messageBrokerSettings.InstanceId}";
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

        services.AddScoped<ISchemaPublisher, SchemaPublisher>();
        services.AddScoped<ISearchIndexPublisher, SearchIndexPublisher>(); 

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IReadUnitOfWork, ReadUnitOfWork>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<ICurrencyReadRepository, CurrencyReadRepository>();
        services.AddScoped<IBankReadRepository, BankReadRepository>();
        services.AddScoped<ICounterpartyReadRepository, CounterpartyReadRepository>();
        services.AddScoped<ITransactionReadRepository, TransactionReadRepository>();

        services.AddScoped<IWriteUnitOfWork, WriteUnitOfWork>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        services.AddScoped<ICurrencyWriteRepository, CurrencyWriteRepository>();
        services.AddScoped<IBankWriteRepository, BankWriteRepository>();
        services.AddScoped<ICounterpartyWriteRepository, CounterpartyWriteRepository>();
        services.AddScoped<ITransactionWriteRepository, TransactionWriteRepository>();

        return services;
    }
}
