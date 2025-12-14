using Microsoft.OpenApi;
using Onefocus.Common;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure;
using Onefocus.Common.Utilities;
using Onefocus.Search.Api.Endpoints;
using Onefocus.Search.Application;
using Onefocus.Search.Infrastructure;
using Onefocus.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

var preLoggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
});
var logger = preLoggerFactory.CreateLogger("SearchApiLog");
builder.Services.AddSingleton(preLoggerFactory);

builder.AddServiceDefaults();
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHttpContextAccessor();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onefocus Search", Description = Common.SwaggerApiInfoDescription, Version = "v1" });
    option.DocumentFilter<SwaggerDocumentFilter>(new KeyValuePair<string, string>[] {
        new ("default", "/"),
        new ("with gateway", "/search")
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

services.AddInfrastructure(logger, configuration).AddApplication();

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

app.MapSearchEndpoints();

app.Run();