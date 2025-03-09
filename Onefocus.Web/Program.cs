using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
}

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Client/wallet/dist")),
    RequestPath = "/wallet"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Client/wallet/dist")),
    RequestPath = "/wallet"
});
app.UseRouting();

app.MapGet("/wallet", () =>
{
    return Results.Redirect("/wallet/index.html");
});

app.MapFallbackToFile("/wallet/index.html"); 

app.Run();
