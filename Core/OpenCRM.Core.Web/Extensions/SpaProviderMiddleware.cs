using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace OpenCRM.Core.Web.Extensions;

public static class SpaProviderMiddleware
{
    public static WebApplication UseSpaProvider(this WebApplication app, string spaRoot, string? requestPath = "/")
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (string.IsNullOrWhiteSpace(spaRoot))
        {
            return app;
        }

        var resolvedSpaRoot = Path.IsPathRooted(spaRoot)
            ? spaRoot
            : Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, spaRoot));

        if (Directory.Exists(resolvedSpaRoot))
        {
            var spaFileProvider = new PhysicalFileProvider(resolvedSpaRoot);
            var normalizedRequestPath = NormalizeRequestPath(requestPath);

            void ConfigureSpaPipeline(IApplicationBuilder builder)
            {
                builder.UseDefaultFiles(new DefaultFilesOptions
                {
                    FileProvider = spaFileProvider
                });
                builder.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = spaFileProvider
                });

                builder.Run(async context =>
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

            if (normalizedRequestPath == "/")
            {
                ConfigureSpaPipeline(app);
            }
            else
            {
                app.Map(normalizedRequestPath, ConfigureSpaPipeline);
            }
        }

        return app;
    }

    private static string NormalizeRequestPath(string? requestPath)
    {
        if (string.IsNullOrWhiteSpace(requestPath))
        {
            return "/";
        }

        if (!requestPath.StartsWith("/"))
        {
            requestPath = "/" + requestPath;
        }

        if (requestPath.Length > 1 && requestPath.EndsWith("/"))
        {
            requestPath = requestPath.TrimEnd('/');
        }

        return requestPath;
    }
}
