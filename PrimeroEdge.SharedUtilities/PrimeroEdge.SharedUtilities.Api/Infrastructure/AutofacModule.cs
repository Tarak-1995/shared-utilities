
/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Autofac;
using Autofac.Features.Indexed;
using AutoMapper;
using Cybersoft.Platform.Data.MongDb;
using Cybersoft.Platform.Data.Sql;
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
        }
    }
}
