using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using OpenCRM.Core.Web;
using OpenCRM.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("SpaCors", policy =>
    {
        policy.WithOrigins("http://localhost:9000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddOpenCRM<OpenCRMDataContext>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("SpaCors");
app.UseStaticFiles();
app.UseOpenCRM<OpenCRMDataContext>();

app.UseHttpsRedirection();

app.MapControllers();

// Serve Angular UI from Core/OpenCRM.Core.Web/ui
var spaRoot = Path.Combine(app.Environment.ContentRootPath, "Core", "OpenCRM.Core.Web", "ui");
if (Directory.Exists(spaRoot))
{
    var spaFileProvider = new PhysicalFileProvider(spaRoot);
    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = spaFileProvider
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = spaFileProvider
    });

    app.MapFallback(async context =>
    {
        var indexFile = spaFileProvider.GetFileInfo("index.html");
        if (indexFile.Exists)
        {
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync(indexFile);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
    });
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
