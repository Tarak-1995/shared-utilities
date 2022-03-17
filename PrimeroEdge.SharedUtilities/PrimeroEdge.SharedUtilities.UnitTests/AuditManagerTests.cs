/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using NSubstitute;
using NUnit.Framework;
using PrimeroEdge.SharedUtilities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.UnitTests
{
	public class AuditManagerTests
	{
		private IAuditManager _auditManager;
		private IAuditRepository _auditRepository;

		private int regionId;
		private string moduleId;
		private string entityTypeId;
		private string entityId;
		private string oldValue;
		private string newValue;
		private string comment;
		private int createdBy;
		private string field;
		private DateTime createDate;
		private string userName;
		private Guid auditId;

		[SetUp]
		public void SetUp()
		{
			this._auditRepository = Substitute.For<IAuditRepository>();

			_auditManager = new AuditManager(_auditRepository);

			auditId = Guid.NewGuid();
			regionId = 54;
			field = "Types";
			moduleId = "SYSTEM";
			entityTypeId = "SYSTEMS-NOTIFICATIONS";
			entityId = "615";
			oldValue = "Channel";
			newValue = "EMAIL";
			comment = "";
			createdBy = 4742;
			createDate = new DateTime(2022, 02, 01);
			userName = "Monroe User";
		}

		[TearDown]
		public void TearDown()
		{
			_auditRepository = null;
			_auditManager = null;
		}


		[Test]
		public async Task GetAuditDataAsyncTest()
		{
			var pageData = BuildMockAuditData();
			var timeZoneSettings = BuildMockTimeZoneSettings();
			var users = BuildMockUsers();

			_auditRepository.GetAuditDataAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
				Arg.Any<int>(), Arg.Any<int>(),
				Arg.Any<int>()).ReturnsForAnyArgs(pageData);

			_auditRepository.GetTimeZoneSettingsAsync(Arg.Any<int>()).ReturnsForAnyArgs(timeZoneSettings);
			_auditRepository.GetUsersAsync(Arg.Any<List<int>>()).ReturnsForAnyArgs(users);

			var result = await _auditManager.GetAuditDataAsync(moduleId, entityTypeId, entityId, 10, 1, regionId);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(field, result.FirstOrDefault()?.Field);
			Assert.AreEqual(userName, result.FirstOrDefault()?.UserName);
			Assert.AreEqual(oldValue, result.FirstOrDefault()?.OldValue);
			Assert.AreEqual(newValue, result.FirstOrDefault()?.NewValue);
		}

		[Test]
		public async Task GetAuditSearchDataAsyncWithFiltersTest()
		{

			var pageData = BuildMockAuditData();
			var timeZoneSettings = BuildMockTimeZoneSettings();
			var users = BuildMockUsers();

			_auditRepository.GetAuditSearchDataAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
				Arg.Any<int>(), Arg.Any<int>(),
				Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(pageData);

			_auditRepository.GetTimeZoneSettingsAsync(Arg.Any<int>()).ReturnsForAnyArgs(timeZoneSettings);
			_auditRepository.GetUsersAsync(Arg.Any<List<int>>()).ReturnsForAnyArgs(users);

			var result = await _auditManager.GetAuditDataSearchAsync(moduleId,entityTypeId,entityId, 10, 1, regionId,field, userName, createDate);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(field, result.FirstOrDefault()?.Field);
			Assert.AreEqual(userName, result.FirstOrDefault()?.UserName);
			Assert.AreEqual(oldValue, result.FirstOrDefault()?.OldValue);
			Assert.AreEqual(newValue, result.FirstOrDefault()?.NewValue);
		}


		[Test]
		public async Task GetAuditSearchDataAsyncWithoutFiltersTest()
		{
			//Arrange
			var pageData = BuildMockAuditData();
			var timeZoneSettings = BuildMockTimeZoneSettings();
			var users = BuildMockUsers();

			_auditRepository.GetAuditSearchDataAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
				Arg.Any<int>(), Arg.Any<int>(),
				Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(pageData);

			_auditRepository.GetTimeZoneSettingsAsync(Arg.Any<int>()).ReturnsForAnyArgs(timeZoneSettings);
			_auditRepository.GetUsersAsync(Arg.Any<List<int>>()).ReturnsForAnyArgs(users);

			//Act
			var result = await _auditManager.GetAuditDataSearchAsync(moduleId, entityTypeId, entityId, 10, 1, regionId, null, null, null);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(field, result.FirstOrDefault()?.Field);
			Assert.AreEqual(userName, result.FirstOrDefault()?.UserName);
			Assert.AreEqual(oldValue, result.FirstOrDefault()?.OldValue);
			Assert.AreEqual(newValue, result.FirstOrDefault()?.NewValue);
		}

		[Test]
		public async Task SaveAuditGroupDataAsyncTest()
		{
			await _auditManager.SaveAuditDataAsync(new List<AuditGroupRequest>(), "Module1", "typeId", "Id", 1, 1);
			Assert.IsTrue(true);
		}

		[Test]
		public async Task SaveAuditDataAsyncTest()
		{
			await _auditManager.SaveAuditDataAsync(new List<AuditRequest>(), "Module1", "typeId", "Id", 1, 1);
			Assert.IsTrue(true);
		}

		private Tuple<List<Audit>, int> BuildMockAuditData()
		{
			var pageData = new List<Audit>()
			{
				new Audit()
				{
					AuditId = Guid.NewGuid(),
					CreatedBy = createdBy,
					CreatedDate = createDate,
					EntityId = entityId,
					EntityTypeId = entityTypeId,
					Field = field,
					Comment = comment,
					RegionId = regionId,
					ModuleId = moduleId,
					OldValue = oldValue,
					NewValue = newValue,
				}
			};

			var count = 1;
			return Tuple.Create(pageData, count);
		}

		private Tuple<string, bool> BuildMockTimeZoneSettings()
		{
			return Tuple.Create("Central Standard Time", true);
		}

		private Dictionary<int, string> BuildMockUsers()
		{
			var userDetails = new Dictionary<int, string>()
			{
				{createdBy, userName}
			};

			return userDetails;
		}

	}
}
