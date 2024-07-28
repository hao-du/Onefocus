using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Onefocus.Identity.Domain.DbContexts;
using Onefocus.Identity.Domain.Entities;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorization();

builder.Services.AddIdentityCore<User>()
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddApiEndpoints();

builder.Services.AddDbContext<IdentityDbContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDatabase")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<User>();

app.MapGet("/get", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}")
    .RequireAuthorization();

app.Run();
