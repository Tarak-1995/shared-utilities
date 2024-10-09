/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using Autofac;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Cybersoft.Platform.Configuration.KeyVaultConfigurationProvider;

namespace PrimeroEdge.SharedUtilities.Api
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        public static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        public static string AppName => typeof(Program).Assembly.GetName().Name;
        public static Version AppVersion => typeof(Program).Assembly.GetName().Version;
        public static bool IsOpenApi
        {
            get
            {
                string openapi = Environment.GetEnvironmentVariable("OPENAPI") ?? "False";
                return openapi.Equals("True", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Env}.json", optional: true)
            .AddJsonFile("settings/appsettings.secrets.json", optional: true, reloadOnChange: true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
            .AddEnvironmentVariables()
            .Build();

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// CreateHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args) 
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureAppConfiguration(builder =>
                    {
                        //
                        // add keyvault configuration provider.
                        //
                        builder.AddKeyVaultConfigurationSource();
                    })
                    .ConfigureContainer<ContainerBuilder>(builder =>
                    {
                        builder.RegisterModule(new AutofacModule());
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>()
                                    .UseConfiguration(Configuration);
                    });
    }
}
