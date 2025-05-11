using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Configurations;
using Onefocus.Wallet.Domain.Repositories.Read;
using Onefocus.Wallet.Domain.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Write;
using System.Runtime.Serialization;

namespace Onefocus.Wallet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var readDatabaseConnectionString = configuration.GetConnectionString("WalletReadDatabase");
        var writeDatabaseConnectionString = configuration.GetConnectionString("WalletWriteDatabase");

        services.AddDbContext<WalletReadDbContext>(option =>
        {
            option.UseNpgsql(readDatabaseConnectionString);
            option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        services.AddDbContext<WalletWriteDbContext>(option =>
            option.UseNpgsql(writeDatabaseConnectionString)
        );

        services.AddScoped<IReadUnitOfWork, ReadUnitOfWork>();
        services.AddScoped<IWriteUnitOfWork, WriteUnitOfWork>();

        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();

        services.AddScoped<ICurrencyReadRepository, CurrencyReadRepository>();
        services.AddScoped<ICurrencyWriteRepository, CurrencyWriteRepository>();

        return services;
    }
}
