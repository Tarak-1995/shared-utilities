/*
 ***********************************************************************
 * Copyright � 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Configuration;
using System.IO;
using Cybersoft.Platform.Authorization.HeaderUtilities.Extensions;
using Cybersoft.Platform.Authorization.HeaderUtilities.Factories;
using Cybersoft.Platform.Cache.CacheObjects;
using Cybersoft.Platform.Cache.CacheObjects.Contracts;
using Cybersoft.Platform.Cache.Extensions;
using Cybersoft.Platform.Couchbase.Client;
using Cybersoft.Platform.Couchbase.Extensions;
using Cybersoft.Platform.AppSettings.Services;
using Cybersoft.Platform.AppSettings.Azure;
using Cybersoft.Platform.DocumentStorage.AzureTable;
using Cybersoft.Platform.DocumentStorage.Settings;
using Cybersoft.Platform.Utilities.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PrimeroEdge.SharedUtilities.Components.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using TableStorage.Abstractions.Store;

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shared Utilities", Version = "v6" });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            if (Program.IsOpenApi)
            {
                //
                // Swashbuckle CLI bootstrapper will return here without loading all connfigurations
                //
                return;
            }

            var bypassAuthSettings = Options.Create(Configuration.GetSection("BypassAuthenticationSettings").Get<BypassAuthenticationSettings>());
            services.AddSessionFactory(bypassAuthSettings.Value);

            services.AddCouchbase(Configuration);
            services.AddRedisCache(Configuration);
            services.AddSingleton<HttpStatusMessageFactory>();
            services.AddTransient<IAzureTableStorage<AuditLogEntity>>(provider =>
            {
                var connString = Configuration.GetSection("AzureBlobStorageCredential").Get<AzureBlobStorageCredential>().ConnectionString;
                return new AzureTableStorage<AuditLogEntity>("AuditLogs", connString, provider.GetService<ILogger<AzureTableStorage<AuditLogEntity>>>());
            });


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
