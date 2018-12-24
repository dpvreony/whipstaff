﻿namespace Dhgms.AspNetCoreContrib.Example.WebSite
{
    using System;
    using Audit.WebApi;
    using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm;
    using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.HealthChecks;
    using Dhgms.AspNetCoreContrib.Fakes;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using OwaspHeaders.Core.Extensions;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var fakeControllerAssembly = typeof(FakeCrudController).Assembly;

            services.AddMvc().AddApplicationPart(fakeControllerAssembly);
            services.AddMediatR(fakeControllerAssembly);

            new HealthChecksApplicationStartHelper().ConfigureService(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.OperationFilter<SwaggerClassMetaDataOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var version = new Version(0, 1, 1, 9999);
            ApmApplicationStartHelper.Configure(this.Configuration, app, env, version);

            var secureHeadersMiddlewareConfiguration = SecureHeadersMiddlewareExtensions.BuildDefaultConfiguration();
            app.UseSecureHeadersMiddleware(secureHeadersMiddlewareConfiguration);

            app.UseStaticFiles();

            app.UseAuditMiddleware(_ => _
                .FilterByRequest(rq => !rq.Path.Value.EndsWith("favicon.ico", StringComparison.OrdinalIgnoreCase))
                .WithEventType("{verb}:{url}")
                .IncludeHeaders()
                .IncludeRequestBody()
                .IncludeResponseBody());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
