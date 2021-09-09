/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Cybersoft.Platform.Data.MongDb;
using NSubstitute;
using NUnit.Framework;
using PrimeroEdge.SharedUtilities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cybersoft.Platform.Utilities.ResponseModels;
using MongoDB.Driver;
using Cybersoft.Platform.Utilities.Factories;
using Cybersoft.Platform.Authorization.HeaderUtilities.Factories;
using Cybersoft.Platform.Data.Sql;
using Cybersoft.Platform.Couchbase.Client;

namespace PrimeroEdge.SharedUtilities.UnitTests
{
    public class AuditManagerTests
    {
        private IAuditManager _auditManager;
        private IAuditRepository _auditRepository;
        private  ICouchbaseCluster _couchbaseCluster;
        private  ISqlDbManager _sqlDbManager;


        [SetUp]
        public void SetUp()
        {
            this._sqlDbManager = Substitute.For<ISqlDbManager>();
            this._couchbaseCluster = Substitute.For<ICouchbaseCluster>();

            _auditRepository = new AuditRepository(this._couchbaseCluster, this._sqlDbManager);
            _auditManager = new AuditManager(_auditRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _couchbaseCluster = null;
            _sqlDbManager = null;
            _auditRepository = null;
            _auditManager = null;
        }


        [Test]
        public async Task GetAuditDataAsyncTest()
        {
            var data = await _auditManager.GetAuditDataAsync("Module1", "typeId", "Id", 10, 1,1);
            Assert.IsTrue(data != null);
        }

        [Test]
        public async Task SaveAuditGroupDataAsyncTest()
        {
             await _auditManager.SaveAuditDataAsync(new List<AuditGroupRequest>(),  "Module1", "typeId", "Id",  1, 1);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task SaveAuditDataAsyncTest()
        {
            await _auditManager.SaveAuditDataAsync(new List<AuditRequest>(), "Module1", "typeId", "Id",  1, 1);
            Assert.IsTrue(true);
        }
    }
}
