using Microsoft.OpenApi;
using Onefocus.Common;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure;
using Onefocus.Common.Utilities;
using Onefocus.Identity.Api.Endpoints;
using Onefocus.Identity.Application;
using Onefocus.Identity.Application.Interfaces.Repositories;
using Onefocus.Identity.Application.Interfaces.Services;
using Onefocus.Identity.Infrastructure;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Onefocus.Identity.Infrastructure.Services;
using Onefocus.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHttpContextAccessor();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onefocus Identity", Description = Common.SwaggerApiInfoDescription, Version = "v1" });
    option.DocumentFilter<SwaggerDocumentFilter>(new KeyValuePair<string, string>[] {
        new ("default", "/"),
        new ("with gateway", "/api/identity")
    });
});

services.AddAuthenticationSettings(configuration);
services.AddAuthorization();

services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ITokenRepository, TokenRepository>();

services
    .AddInfrastructure(configuration)
    .AddApplication();

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

app.MapAuthenticationEndpoints();

app.Run();