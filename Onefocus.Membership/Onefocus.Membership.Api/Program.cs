using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Onefocus.Common;
using Onefocus.Common.Infrastructure;
using Onefocus.Membership.Api.Endpoints;
using Onefocus.Membership.Api.Security;
using Onefocus.Membership.Application;
using Onefocus.Membership.Infrastructure;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onefocus", Description = "Manage everything in one place!", Version = "v1" });
    option.DocumentFilter<SwaggerDocumentFilter>("/membership");
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
            new string[]{}
        }
    });
});

builder.Services.AddAuthenticationSettings(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapUserEndpoints();

app.Run();