using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using OpenCRM.Core.Web;
using OpenCRM.Core.Web.Middlewares;
using OpenCRM.SwissLPD;
using OpenCRM.Web.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenCRM<OpenCRMDataContext>(builder.Configuration);
builder.Services.AddOpenCRMSwissLPD<OpenCRMDataContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Globalization and Localization
builder.Services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
#endregion



var app = builder.Build();

app.UseMiddleware<SpaMiddleware>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
app.UseForwardedHeaders(new ForwardedHeadersOptions  { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseSpaMiddleware();
app.UseRouting();
app.UseAuthentication();

#region Globalization and Localization
var supportedCultures = new[] { "en","fr"};
var optionLocalization = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(optionLocalization);
#endregion

app.UseOpenCRM<OpenCRMDataContext>();
app.UseOpenCRMSwissLPDAsync<OpenCRMDataContext>();
app.MapRazorPages();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();
