/*
 ***********************************************************************
 * Copyright � 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.IO;
using Cybersoft.Platform.Authorization.HeaderUtilities.Extensions;
using Cybersoft.Platform.Authorization.HeaderUtilities.Factories;
using Cybersoft.Platform.Couchbase.Client;
using Cybersoft.Platform.Couchbase.Settings;
using Cybersoft.Platform.Utilities.MiddleWare;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PrimeroEdge.SharedUtilities.Api
{

    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shared Utilities", Version = "v1" });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
            var bypassAuthSettings = Options.Create(Configuration.GetSection("BypassAuthenticationSettings").Get<BypassAuthenticationSettings>());
            services.AddSessionFactory(bypassAuthSettings.Value);

            services.Configure<CouchbaseSettings>(options => this.Configuration.GetSection("AuditCouchbaseSettings").Bind(options));
            var couchbaseOptions = Options.Create(this.Configuration.GetSection("AuditCouchbaseSettings").Get<CouchbaseSettings>());
            var couchbaseCluster = CouchbaseClusterFactory.Build(couchbaseOptions).Result;
            services.AddSingleton<ICouchbaseCluster>(_ => couchbaseCluster);
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseSessionMiddleware();
            app.UseMiddleware<ResponseMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            var vpath = Configuration["SubDomain"];

            if (!string.IsNullOrWhiteSpace(vpath))
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = $"swagger/{{documentName}}/swagger.json";
                });
                app.UseSwaggerUI(options =>
                {
                    options.DocExpansion(DocExpansion.None);
                    options.SwaggerEndpoint($"/{vpath}/swagger/v1/swagger.json", name: "Shared Utilities API");
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocExpansion(DocExpansion.None);
                    c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Shared Utilities API");
                });
            }
        }
    }
}
