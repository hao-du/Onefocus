using Microsoft.AspNetCore.Identity;
using Onefocus.Identity.Infrastructure;
using Onefocus.Identity.Application;
using Onefocus.Identity.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onefocus.Identity.Api.Endpoints;
using Onefocus.Common;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Onefocus.Common.Infrastructure;
using System.Security.Principal;
using Onefocus.Common.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onefocus Identity", Description = Commons.SwaggerApiInfoDescription, Version = "v1" });
    option.DocumentFilter<SwaggerDocumentFilter>(new KeyValuePair<string, string>[] {
        new ("default", "/"),
        new ("with gateway", "/identity")
    });
});

builder.Services.AddAuthenticationSettings(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .AddInfrastructure(builder.Configuration)
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