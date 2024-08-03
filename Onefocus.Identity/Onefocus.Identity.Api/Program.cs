using Microsoft.AspNetCore.Identity;
using Onefocus.Identity.Infrastructure;
using Onefocus.Identity.Application;
using Onefocus.Identity.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onefocus.Identity.Api.Endpoints;
using Onefocus.Membership.Api.Security;
using Onefocus.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.MapIdentityApi<User>();

app.Run();
