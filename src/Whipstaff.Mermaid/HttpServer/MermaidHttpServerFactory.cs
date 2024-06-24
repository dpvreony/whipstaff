﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
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
            _ = applicationBuilder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = embeddedFileProvider
            });

            _ = applicationBuilder.MapWhen(IsMermaidPost, AppConfiguration);
        }

        private static void AppConfiguration(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(Handler);
        }

        private static async Task Handler(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            var diagramFormStringValues = request.Form["diagram"];
            if (diagramFormStringValues.Count < 1)
            {
                await WriteNoDiagramResponse(response).ConfigureAwait(false);
                return;
            }

            var diagram = diagramFormStringValues[0];
            if (string.IsNullOrWhiteSpace(diagram))
            {
                await WriteNoDiagramResponse(response).ConfigureAwait(false);
                return;
            }

            response.StatusCode = 200;
            response.ContentType = "text/html";

            var sb = new System.Text.StringBuilder();
            _ = sb.AppendLine(@"<!DOCTYPE html>");
            _ = sb.AppendLine(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">");
            _ = sb.AppendLine(@"<head>");
            _ = sb.AppendLine(@"    <meta charset=""utf-8"" />");
            _ = sb.AppendLine(@"    <title>MermaidJS factory</title>");
            _ = sb.AppendLine(@"</head>");
            _ = sb.AppendLine(@"<body>");
            _ = sb.AppendLine(@"    <pre class=""mermaid"" name=""mermaid-element"" id=""mermaid-element"">");
            _ = sb.AppendLine(HtmlEncoder.Default.Encode(diagram));
            _ = sb.AppendLine(@"    </pre>");
            _ = sb.AppendLine(@"    <script type=""module"">");
            _ = sb.AppendLine(@"        import mermaid from '/lib/mermaid/mermaid.esm.min.mjs';");
            _ = sb.AppendLine(@"        mermaid.initialize({ startOnLoad: false });");
            _ = sb.AppendLine(@"        await mermaid.run();");
            _ = sb.AppendLine(@"    </script>");
            _ = sb.AppendLine(@"</body>");
            _ = sb.AppendLine(@"</html>");

            await response.WriteAsync(sb.ToString())
                .ConfigureAwait(false);
        }

        private static async Task WriteNoDiagramResponse(HttpResponse response)
        {
            response.StatusCode = 400;
            response.ContentType = "text/plain";
            await response.WriteAsync("No diagram passed in request").ConfigureAwait(false);
        }

        private static bool IsMermaidPost(HttpContext arg)
        {
            var request = arg.Request;

            return request.Method.Equals("POST", StringComparison.Ordinal) &&
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
