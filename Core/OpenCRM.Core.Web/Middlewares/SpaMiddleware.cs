using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace OpenCRM.Core.Web.Middlewares
{
    public static class SpaMiddlewareExtensions
    {
        public static string GetSpaPath()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var parentDir = Directory.GetParent(currentDir);
            if (parentDir == null) return string.Empty;
            return Path.Combine(parentDir.FullName, "UI", "OpenCRM.Web.UI", "dist", "OpenCRM.Web.UI", "browser");
        }

        public static IApplicationBuilder UseSpaMiddleware(this IApplicationBuilder app)
        {
            var spaPath = GetSpaPath();
            if (string.IsNullOrEmpty(spaPath) || !Directory.Exists(spaPath)) return app;

            app.UseMiddleware<SpaMiddleware>();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(spaPath),
                RequestPath = "/ui",
            });

            return app;
        }
    }

    public class SpaMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public SpaMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path != null && (path == "/ui" || path == "/ui/"))
            {
                var spaPath = SpaMiddlewareExtensions.GetSpaPath();
                var filePath = Path.Combine(_env.WebRootPath, spaPath, "index.html");

                if (File.Exists(filePath))
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(filePath);
                    return;
                }
            }
            await _next(context);
        }
    }
}
