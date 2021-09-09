
/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Autofac;
using Cybersoft.Platform.Configuration;
using Cybersoft.Platform.Data.MongDb;
using Cybersoft.Platform.Utilities;
using PrimeroEdge.SharedUtilities.Components;
using System;
using System.Threading.Tasks;
using Cybersoft.Platform.Contracts;
using Cybersoft.Platform.KeyVault;
using Cybersoft.Platform.Logging;
using Cybersoft.Platform.Utilities.ResponseModels;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using AutoMapper.Data;
using Cybersoft.Platform.Data.Sql;

namespace PrimeroEdge.SharedUtilities.Api
{

    /// <summary>
    /// AutofacModule
    /// </summary>
    public class AutofacModule : Autofac.Module
    {

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            var mapper = new MapperConfiguration(mc =>
            {
                mc.AddDataReaderMapping();
            }).CreateMapper();

            builder.Register<IMapper>(c => mapper).InstancePerLifetimeScope();

            //Configuration settings
            builder.AddConfiguration();
            builder.ConfigureKeyVault();

            //Audit settings
            builder.RegisterType<AuditManager>().As<IAuditManager>().SingleInstance();
            builder.RegisterType<AuditRepository>().As<IAuditRepository>().SingleInstance();

            // Connection strings
            builder.RegisterSettings<ConnectionStrings>(ConfigKeys.ConnectionStrings);
            builder.Register<ISqlDbManager>((c) =>
            {
                var connectionStrings = c.Resolve<Lazy<Task<ConnectionStrings>>>().Value.Result;
                var iMapper = c.Resolve<IMapper>();
                var connString = c.DecryptKeyVaultString(CryptoManager.CONNECTION_STRING_KEY, connectionStrings.Connections[ConnectionType.ADMINISTRATION.ToString()]);
                return new SqlDbManager(connString, iMapper);
            }).SingleInstance();

            builder.RegisterSettings<ErrorMessageSettings>(ConfigKeys.ErrorMessageSettings);
            builder.Register<IMongoDbManager<MessageData>>(c =>
            {
                var settings = c.Resolve<Lazy<Task<ErrorMessageSettings>>>().Value.Result;
                settings.MongoDbSettings.ConnectionString = c.DecryptKeyVaultString(CryptoManager.CONNECTION_STRING_KEY, settings.MongoDbSettings.ConnectionString);
                return new MongoDbManager<MessageData>(settings.MongoDbSettings);
            }).SingleInstance();


            builder.Register<ICybersoftLogger>(c =>
            {
                var settings = c.Resolve<IConfiguration>().GetSection("LogSettings").Get<LogSettings>();
                return LoggerFactory.GetLogger(settings);
            });

        }
    }
}
