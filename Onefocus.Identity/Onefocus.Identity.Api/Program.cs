using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Onefocus.Common;
using Onefocus.Common.Configurations;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure;
using Onefocus.Identity.Api.Endpoints;
using Onefocus.Identity.Application;
using Onefocus.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onefocus Identity", Description = Commons.SwaggerApiInfoDescription, Version = "v1" });
    option.DocumentFilter<SwaggerDocumentFilter>(new KeyValuePair<string, string>[] {
        new ("default", "/"),
        new ("with gateway", "/identity")
    });
});

services.AddAuthenticationSettings(configuration);
services.AddAuthorization();

services
    .AddInfrastructure(configuration)
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthenticationEndpoints();

app.Run();