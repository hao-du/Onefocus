using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapStaticAssets();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Client/dist/assets")),
    RequestPath = "/assets",
});

app.UseRouting();

app.MapControllerRoute(
    name: "catch-all",
    pattern: "{**url}",
    defaults: new { controller = "Home", action = "Index" }
);

app.Run();
