using Microsoft.OpenApi;
using Onefocus.Common;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure;
using Onefocus.Common.Utilities;
using Onefocus.ServiceDefaults;
using Onefocus.Wallet.Api.Endpoints;
using Onefocus.Wallet.Application;
using Onefocus.Wallet.Infrastructure;

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
    option.AddSecurityRequirement(document => new() { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] });
});

services.AddAuthenticationSettings(configuration);
services.AddAuthorization();

services.AddInfrastructure(configuration).AddApplication();

services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();

var app = builder.Build();

await app.UseSearch();

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