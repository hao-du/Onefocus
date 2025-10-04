using Microsoft.OpenApi.Models;
using Onefocus.Common;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure;
using Onefocus.Common.Utilities;
using Onefocus.Home.Api.Endpoints;
using Onefocus.Home.Application;
using Onefocus.Home.Application.Interfaces.Repositories.Read;
using Onefocus.Home.Application.Interfaces.Repositories.Write;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Home.Infrastructure;
using Onefocus.Home.Infrastructure.Databases.Repositories.Read;
using Onefocus.Home.Infrastructure.Databases.Repositories.Write;
using Onefocus.Home.Infrastructure.UnitOfWork.Read;
using Onefocus.Home.Infrastructure.UnitOfWork.Write;
using Onefocus.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHttpContextAccessor();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onefocus Home", Description = Common.SwaggerApiInfoDescription, Version = "v1" });
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
    .AddInfrastructure(configuration)
    .AddApplication(configuration);

services.AddScoped<IReadUnitOfWork, ReadUnitOfWork>();
services.AddScoped<ISettingReadRepository, SettingReadRepository>();

services.AddScoped<IWriteUnitOfWork, WriteUnitOfWork>();
services.AddScoped<IUserWriteRepository, UserWriteRepository>();
services.AddScoped<ISettingWriteRepository, SettingWriteRepository>();

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
app.MapSettingEndpoints();

app.Run();