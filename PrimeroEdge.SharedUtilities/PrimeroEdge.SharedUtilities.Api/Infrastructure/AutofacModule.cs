
/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Text;
using Autofac;
using Autofac.Features.Indexed;
using AutoMapper;
using Cybersoft.Platform.Data.MongDb;
using Cybersoft.Platform.Data.Sql;
using Cybersoft.Platform.Message.Publisher;
using Newtonsoft.Json;
using PrimeroEdge.SharedUtilities.Components;

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

            builder.RegisterGeneric(typeof(MongoDbManager<>)).As(typeof(IMongoDbManager<>)).InstancePerLifetimeScope();
            builder.RegisterType<AuditManager>().As<IAuditManager>().InstancePerLifetimeScope();
            builder.RegisterType<AuditRepository>().As<IAuditRepository>().InstancePerLifetimeScope();

            var mapper = new MapperConfiguration(mc =>
            {
            }).CreateMapper();
            builder.Register<IMapper>(c => mapper).InstancePerLifetimeScope();

            builder.RegisterType<BlobStorageRepository>().Keyed<IFileStorageRepository>(FileStorageType.BlobStorage).InstancePerLifetimeScope();
            builder.RegisterType<FileShareStorageRepository>().Keyed<IFileStorageRepository>(FileStorageType.FlieShare).InstancePerLifetimeScope();
            builder.RegisterType<SqldbStorageRepository>().Keyed<IFileStorageRepository>(FileStorageType.SqlDataBase).InstancePerLifetimeScope();

            builder.Register<IFileStorageManager>(c =>
            {
                var fileStorageSettings = c.Resolve<FileStorageSettings>();
                var rep = c.Resolve<IIndex<FileStorageType, IFileStorageRepository>>()[fileStorageSettings.StorageType];
                return new FileStorageManager(rep);
            }).InstancePerLifetimeScope();

            builder.RegisterType<SqlDbManager>().As<ISqlDbManager>().InstancePerLifetimeScope();

            var messageQueueSettings = JsonConvert.DeserializeObject<MessageQueueSettings>(LoadMessagingQueueSettings());
            //builder.Register<IPublisher>(c => GetDependency(messageQueueSettings.PublisherAssembly, Convert.ToString(messageQueueSettings.MessageProviderSettings)));

            builder.Register<IPublisher>(c =>
            {
                //if (messageQueueSettings.MessagingProvider == "ServiceBus")
                //{
                //   // ServiceBusHelper.Instance.Init(Convert.ToString(messageQueueSettings.MessageProviderSettings));

                //    return new AzureServiceBusPublisher(Convert.ToString(messageQueueSettings.MessageProviderSettings));
                //}
                //else if (messageQueueSettings.MessagingProvider == "RabbitMQ")
                //{
                //    // RabbitMQHelper.Instance.Init(Convert.ToString(messageQueueSettings.MessageProviderSettings));

                //    return GetDependency(messageQueueSettings.PublisherAssembly, Convert.ToString(messageQueueSettings.MessageProviderSettings));

                //   // return new RabbitMQPublisher(Convert.ToString(messageQueueSettings.MessageProviderSettings));
                //}

                //return null;

                return GetDependency(messageQueueSettings.PublisherAssembly, Convert.ToString(messageQueueSettings.MessageProviderSettings));

            });

            //builder.RegisterType<AzureServiceBusPublisher>()
            //.As<IPublisher>().Keyed<IPublisher>(messageQueueSettings.MessagingProvider);

            //builder.RegisterType<RabbitMQPublisher>()
            //  .As<IPublisher>().Keyed<IPublisher>(messageQueueSettings.MessagingProvider);

            builder.RegisterType<MessagePublisher>().AsSelf();

        }

        private IPublisher GetDependency(string assemblyName, params object[] paramArray)
        {
            //  string classToCreate = System.Configuration.ConfigurationManager.AppSettings["ClassName"];

            Type type = System.Type.GetType(assemblyName);
            IPublisher dependency = (IPublisher)Activator.CreateInstance(type, paramArray);
            return dependency;
        }

        private static string LoadMessagingQueueSettings()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("'MessagingProvider': 'RabbitMQ',");
            sb.Append("'PublisherAssembly' : 'Cybersoft.Platform.Message.Publisher.RabbitMQPublisher, Cybersoft.Platform.Message.Publisher, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null',");
            sb.Append("'MessageProviderSettings': {");
            sb.Append("'HostName': 'localhost',"); //Need to change to RabbitMQ hosted server name 
            sb.Append("'Port': 5672,");
            sb.Append("'UserName': 'guest',"); //ToDo 
            sb.Append("'Password': 'guest',"); //ToDo 
            sb.Append("'QueueName': 'CybersoftLog',");//ToDo  
            sb.Append("'RoutingKey': 'CybersoftLog',"); //ToDo  
            sb.Append("'IsDurable': true,");
            sb.Append("'IsExclusive': false,");
            sb.Append("'IsAutoDelete': false");
            sb.Append("}");
            sb.Append("}");

            return sb.ToString();

            //return File.ReadAllText(@"C:\Users\srinivasarao.pepakay\VS2019\RabbitMQMessagePublisherSettings.json");
        }
    }

    /// <summary>
    /// MessageQueueSettings
    /// </summary>
    public class MessageQueueSettings
    {
        /// <summary>
        /// MessagingProvider
        /// </summary>
        public string MessagingProvider { get; set; }
        /// <summary>
        /// PublisherAssembly
        /// </summary>
        public string PublisherAssembly { get; set; }
        /// <summary>
        /// MessageProviderSettings
        /// </summary>
        public dynamic MessageProviderSettings { get; set; }
    }
}
