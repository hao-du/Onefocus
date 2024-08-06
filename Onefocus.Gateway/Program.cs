using Onefocus.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthenticationSettings(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PrivateRoute", policy =>
        policy.RequireAuthenticatedUser());
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
