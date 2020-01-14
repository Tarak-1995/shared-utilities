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

namespace PrimeroEdge.SharedUtilities.UnitTests
{
    public class AuditManagerTests
    {
        private IAuditManager _auditManager;
        private IAuditRepository _auditRepository;
        private IMongoDbManager<Audit> _mongoDbManager;


        [SetUp]
        public void SetUp()
        {
            _mongoDbManager = Substitute.For<IMongoDbManager<Audit>>();
            _auditRepository = new AuditRepository(_mongoDbManager);
            _auditManager = new AuditManager(_auditRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoDbManager = null;
            _auditRepository = null;
            _auditManager = null;
        }

        private readonly List<Audit> _audits = new List<Audit>
        {
            new Audit
            {
                EntityTypeId = EntityType.User,
                EntityId = 1,
                Field ="Test Filed",
                OldValue = "Test Old Value",
                NewValue ="Test new value",
                UserId =103,
            }
        };

        [Test]
        public void AuditManagerConstructor_WhenMissingAuditRepository_ShouldThrowError()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new AuditManager(null));
            Assert.That(exception.ParamName, Is.EqualTo("auditRepository"));
        }

        [Test]
        public void AuditManagerConstructor_WhenAllValidArguments_ShouldReturnInstance()
        {
            Assert.DoesNotThrow(() => new AuditManager(_auditRepository));
        }


        [Test]
        public void AuditRepositoryConstructor_WhenMissingSqlDbManager_ShouldThrowError()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new AuditRepository(null));
            Assert.That(exception.ParamName, Is.EqualTo("mongoDbManager"));
        }

        [Test]
        public void AuditRepositoryConstructor_WhenAllValidArguments_ShouldReturnInstance()
        {
            Assert.DoesNotThrow(() => new AuditRepository(_mongoDbManager));
        }

        [Test]
        public async Task CreateAuditAsyncTest()
        {
            _mongoDbManager.CreateAsync(Arg.Any<List<Audit>>()).Returns(Task.CompletedTask);
            await _auditManager.CreateAuditAsync(_audits).ConfigureAwait(false);
            Assert.IsTrue(true);
        }


        [Test]
        public async Task GetAuditDataAsyncTest()
        {
            _mongoDbManager.QueryAsync(Arg.Any<Expression<Func<Audit, bool>>>()).Returns(Task.FromResult(_audits));
            var data = await _auditManager.GetAuditDataAsync(_audits.First().EntityTypeId, _audits.First().EntityId, "").ConfigureAwait(false);
            Assert.IsTrue(data.Count == _audits.Count);
        }
    }
}
