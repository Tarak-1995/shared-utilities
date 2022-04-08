
/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Autofac;
using PrimeroEdge.SharedUtilities.Components;
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

            //Audit settings
            builder.RegisterType<AuditManager>().As<IAuditManager>().SingleInstance();
            builder.RegisterType<AuditRepository>().As<IAuditRepository>().SingleInstance();

            // Connection strings
            builder.Register<ISqlDbManager>((c) =>
            {
                var iMapper = c.Resolve<IMapper>();
                var connString = c.Resolve<IConfiguration>().GetValue<string>("ConnectionStrings:ADMINISTRATION");
                return new SqlDbManager(connString, iMapper);
            }).SingleInstance();
        }
    }
}
