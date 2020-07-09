
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
using Cybersoft.Platform.DocumentStorage;
using Cybersoft.Platform.KeyVault;
using Cybersoft.Platform.Logging;
using Cybersoft.Platform.Message.Publisher;
using Cybersoft.Platform.Utilities.ResponseModels;
using Microsoft.Extensions.Configuration;

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
            //Configuration settings
            builder.AddConfiguration();
            builder.ConfigureKeyVault();
            builder.ConfigureDocumentStorage(0);

            //Audit settings
            builder.RegisterSettings<AuditSettings>(ConfigKeys.AuditSettings);
            builder.RegisterType<AuditManager>().As<IAuditManager>().SingleInstance();
            builder.RegisterType<AuditRepository>().As<IAuditRepository>().SingleInstance();
            builder.Register((c) => {
                var context = c.Resolve<IComponentContext>();
                return new Lazy<Task<IMongoDbManager<Audit>>>(async () => {
                    var auditSettings = await context.Resolve<Lazy<Task<AuditSettings>>>().Value.ConfigureAwait(false);
                    auditSettings.MongoDbSettings.ConnectionString = context.DecryptKeyVaultString(CryptoManager.CONNECTION_STRING_KEY, auditSettings.MongoDbSettings.ConnectionString);
                    return new MongoDbManager<Audit>(auditSettings.MongoDbSettings);
                });
            }).SingleInstance();


            builder.RegisterSettings<ErrorMessageSettings>(ConfigKeys.ErrorMessageSettings);
            builder.Register<IMongoDbManager<MessageData>>(c =>
            {
                var settings = c.Resolve<Lazy<Task<ErrorMessageSettings>>>().Value.Result;
                settings.MongoDbSettings.ConnectionString = c.DecryptKeyVaultString(CryptoManager.CONNECTION_STRING_KEY, settings.MongoDbSettings.ConnectionString);
                return new MongoDbManager<MessageData>(settings.MongoDbSettings);
            }).SingleInstance();

            builder.Register<IPublisher>(c =>
            {
                var settings = c.Resolve<IConfiguration>().GetSection("MessageQueueSettings")
                    .Get<BaseMessageSettings<RabbitMQQueueSettings>>();
                var type = Type.GetType(settings.PublisherAssembly);
                return (IPublisher)Activator.CreateInstance(type, Newtonsoft.Json.JsonConvert.SerializeObject(settings.MessageProviderSettings));
            });

            builder.RegisterType<MessagePublisher>().AsSelf();

            builder.Register<ICybersoftLogger>(c =>
            {
                var settings = c.Resolve<IConfiguration>().GetSection("LogSettings").Get<LogSettings>();
                return LoggerFactory.GetLogger(settings);
            });

        }
    }
}
