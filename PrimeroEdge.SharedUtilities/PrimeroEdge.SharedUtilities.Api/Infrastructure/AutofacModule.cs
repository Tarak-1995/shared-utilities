
/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Autofac;
using Autofac.Features.Indexed;
using AutoMapper;
using Cybersoft.Platform.Configuration;
using Cybersoft.Platform.Data.MongDb;
using Cybersoft.Platform.Data.Sql;
using Cybersoft.Platform.Utilities;
using Microsoft.Extensions.Configuration;
using PrimeroEdge.SharedUtilities.Components;
using System;
using System.Threading.Tasks;
using Cybersoft.Platform.KeyVault;

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


            //File storge settings
            builder.RegisterSettings<FileStorageSettings>(ConfigKeys.FileStorageSettings);
            var mapper = new MapperConfiguration(mc => { }).CreateMapper();
            builder.Register<IMapper>(c => mapper).SingleInstance();
            builder.RegisterType<BlobStorageRepository>().Keyed<IFileStorageRepository>(FileStorageType.BlobStorage).SingleInstance();
            builder.RegisterType<FileShareStorageRepository>().Keyed<IFileStorageRepository>(FileStorageType.FlieShare).SingleInstance();
            builder.RegisterType<SqldbStorageRepository>().Keyed<IFileStorageRepository>(FileStorageType.SqlDataBase).SingleInstance();
            builder.Register((c) => {
                var context = c.Resolve<IComponentContext>();
                return new Lazy<Task<IFileStorageManager>>(async () => {
                    var fileStorageSettings = await context.Resolve<Lazy<Task<FileStorageSettings>>>().Value.ConfigureAwait(false);
                    fileStorageSettings.BlobConnString = context.DecryptKeyVaultString(CryptoManager.CONNECTION_STRING_KEY, fileStorageSettings.BlobConnString);
                    var rep = context.Resolve<IIndex<FileStorageType, IFileStorageRepository>>()[fileStorageSettings.StorageType];
                    return new FileStorageManager(rep);
                });
            }).SingleInstance();

            //Connection strings
            builder.RegisterSettings<ConnectionStrings>(ConfigKeys.ConnectionStrings);
            builder.Register((c) => {
                var context = c.Resolve<IComponentContext>();
                return new Lazy<Task<ISqlDbManager>>(async () => {
                    var connectionStrings = await context.Resolve<Lazy<Task<ConnectionStrings>>>().Value.ConfigureAwait(false);
                    var iMapper = context.Resolve<IMapper>();
                    var connString = context.DecryptKeyVaultString(CryptoManager.CONNECTION_STRING_KEY, connectionStrings.Connections[ConnectionType.ADMINISTRATION.ToString()]);
                    return new SqlDbManager(connString, iMapper);
                });
            }).SingleInstance();
           
        }
    }
}
