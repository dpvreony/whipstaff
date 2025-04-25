// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Mermaid.HttpServer
{
    /// <summary>
    /// Factory for the Mermaid in memory HTTP server.
    /// </summary>
    public static class MermaidHttpServerFactory
    {
        /// <summary>
        /// Gets the In Memory Test Server.
        /// </summary>
        /// <param name="loggerFactory">Logging Factory.</param>
        /// <returns>In memory HTTP server instance.</returns>
        public static MermaidHttpServer GetTestServer(ILoggerFactory loggerFactory)
        {
            var builder = GetWebHostBuilder(loggerFactory);
            var testServer = new MermaidHttpServer(builder);
            return testServer;
        }

        private static IWebHostBuilder GetWebHostBuilder(ILoggerFactory loggerFactory)
        {
            var embeddedProvider = new EmbeddedFileProvider(
                typeof(MermaidHttpServerFactory).Assembly,
                typeof(MermaidHttpServerFactory).Namespace + ".wwwroot");

            var builder = new WebHostBuilder()
                .ConfigureLogging(loggingBuilder => ConfigureLogging(
                    loggingBuilder,
                    loggerFactory))
                .ConfigureServices((_, serviceCollection) => ConfigureServices(
                    serviceCollection,
                    embeddedProvider))
                .Configure((_, applicationBuilder) => ConfigureApp(
                    applicationBuilder,
                    embeddedProvider));

            return builder;
        }

        private static void ConfigureApp(IApplicationBuilder applicationBuilder, EmbeddedFileProvider embeddedFileProvider)
        {
            _ = applicationBuilder.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;

                if (url != null
                    && url.Contains("/lib/mermaid", StringComparison.OrdinalIgnoreCase)
                    && !url.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
                {
                    // rewrite and continue processing
                    context.Request.Path += ".gz";
                }

                await next();
            });

            _ = applicationBuilder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = embeddedFileProvider,
                OnPrepareResponseAsync = OnPrepareResponseAsync,
                HttpsCompression = HttpsCompressionMode.DoNotCompress
            });

            _ = applicationBuilder.MapWhen(
                static httpContext => IsMermaidPost(httpContext),
                static applicationBuilder => AppConfiguration(applicationBuilder));
        }

        private static Task OnPrepareResponseAsync(StaticFileResponseContext arg)
        {
            if (!arg.File.Name.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            var filename = arg.File.Name[..^3];

            var headers = arg.Context.Response.Headers;
            headers["Content-Encoding"] = "gzip";
            if (new FileExtensionContentTypeProvider().TryGetContentType(filename, out var contentType))
            {
                headers["Content-Type"] = contentType;
            }

            return Task.CompletedTask;
        }

        private static void AppConfiguration(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(static applicationBuilder => HandlerAsync(applicationBuilder));
        }

        private static async Task HandlerAsync(HttpContext context)
        {
            var response = context.Response;

            var diagramFormStringValues = request.Form["diagram"];
            if (diagramFormStringValues.Count < 1)
            {
                await WriteNoDiagramResponseAsync(response).ConfigureAwait(false);
                return;
            }

            var diagram = diagramFormStringValues[0];
            if (string.IsNullOrWhiteSpace(diagram))
            {
                await WriteNoDiagramResponseAsync(response).ConfigureAwait(false);
                return;
            }

            response.StatusCode = 200;
            response.ContentType = "text/html";

            var sb = new System.Text.StringBuilder();
            _ = sb.AppendLine("<!DOCTYPE html>");
            _ = sb.AppendLine(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">");
            _ = sb.AppendLine("<head>");
            _ = sb.AppendLine(@"    <meta charset=""utf-8"" />");
            _ = sb.AppendLine("    <title>MermaidJS factory</title>");
            _ = sb.AppendLine("</head>");
            _ = sb.AppendLine("<body>");
            _ = sb.AppendLine(@"    <div id=""mermaid-element""></div>");
            _ = sb.AppendLine(@"    <script type=""module"">");
            _ = sb.AppendLine("        import mermaid from '/lib/mermaid/mermaid.esm.min.mjs';");
            _ = sb.AppendLine("        mermaid.initialize({ startOnLoad: false });");
            _ = sb.AppendLine("        window.renderMermaid = async function (diagram) {");
            _ = sb.AppendLine("            const container = document.getElementById('mermaid-element');");
            _ = sb.AppendLine("            const { svg, bindFunctions } = await mermaid.render('mermaid-graph', diagram);");
            _ = sb.AppendLine("            container.innerHTML = svg;");
            _ = sb.AppendLine("            bindFunctions?.(container);");
            _ = sb.AppendLine("            return svg;");
            _ = sb.AppendLine("        };");
            _ = sb.AppendLine("        window.mermaid = mermaid;");
            _ = sb.AppendLine("    </script>");
            _ = sb.AppendLine("</body>");
            _ = sb.AppendLine("</html>");

            await response.WriteAsync(sb.ToString())
                .ConfigureAwait(false);
        }

        private static async Task WriteNoDiagramResponseAsync(HttpResponse response)
        {
            response.StatusCode = 400;
            response.ContentType = "text/plain";
            await response.WriteAsync("No diagram passed in request").ConfigureAwait(false);
        }

        private static bool IsMermaidPost(HttpContext arg)
        {
            var request = arg.Request;

            return request.Method.Equals("GET", StringComparison.Ordinal) &&
                   request.Path.Equals("/index.html", StringComparison.OrdinalIgnoreCase);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, EmbeddedFileProvider embeddedFileProvider)
        {
            _ = serviceCollection.AddSingleton<IFileProvider>(embeddedFileProvider);
        }

        private static void ConfigureLogging(ILoggingBuilder loggingBuilder, ILoggerFactory loggerFactory)
        {
            _ = loggingBuilder.Services.AddSingleton(loggerFactory);
        }
    }
}
