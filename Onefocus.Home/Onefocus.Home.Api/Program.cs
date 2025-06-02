using Microsoft.OpenApi.Models;
using Onefocus.Common;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure;
using Onefocus.Common.Utilities;
using Onefocus.Home.Api.Endpoints;
using Onefocus.Home.Application;
using Onefocus.Home.Infrastructure;
using Onefocus.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onefocus Home", Description = Commons.SwaggerApiInfoDescription, Version = "v1" });
    option.DocumentFilter<SwaggerDocumentFilter>(new KeyValuePair<string, string>[] {
        new ("default", "/"),
        new ("with gateway", "/home")
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

services
    .AddInfrastructure()
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

app.MapHomeEndpoints();

app.Run();