using Microsoft.OpenApi.Models;
using Onefocus.Common;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure;
using Onefocus.Common.Utilities;
using Onefocus.ServiceDefaults;
using Onefocus.Wallet.Api.Endpoints;
using Onefocus.Wallet.Application;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Infrastructure;
using Onefocus.Wallet.Infrastructure.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Repositories.Write;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Write;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHttpContextAccessor();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onefocus Wallet", Description = Common.SwaggerApiInfoDescription, Version = "v1" });
    option.DocumentFilter<SwaggerDocumentFilter>(new KeyValuePair<string, string>[] {
        new ("default", "/"),
        new ("with gateway", "/Wallet")
    });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter Bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

services.AddAuthenticationSettings(configuration);
services.AddAuthorization();

services.AddInfrastructure(configuration).AddApplication();

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

services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopmentLike())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapBankEndpoints();
app.MapCurrencyEndpoints();
app.MapCounterpartyEndpoints();
app.MapTransactionEndpoints();

app.Run();