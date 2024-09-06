/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Microsoft.AspNetCore.Mvc;
using PrimeroEdge.SharedUtilities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cybersoft.Platform.Utilities.Extensions;
using Cybersoft.Platform.Utilities.ResponseModels;
using Cybersoft.Platform.Authorization.HeaderUtilities.Models;
using Cybersoft.Platform.Authorization.HeaderUtilities.Factories;
using Cybersoft.Platform.Utilities.Exceptions;
using Newtonsoft.Json;
using PrimeroEdge.SharedUtilities.Components.Common;
using System.Drawing;
using PrimeroEdge.SharedUtilities.Components.Models;

namespace PrimeroEdge.SharedUtilities.Api.Controllers
{

    /// <summary>
    /// AuditsController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuditsController : ControllerBase
    {
        private readonly IAuditManager _auditManager;
        private readonly IAuthorizationContext _authContext;

        /// <summary>
        /// AuditsController
        /// </summary>
        /// <param name="auditManager"></param>
        /// <param name="sessionFactory"></param>
        public AuditsController(IAuditManager auditManager, IUserSessionFactory sessionFactory)
        {
            _auditManager = auditManager ?? throw new ArgumentNullException(nameof(auditManager));
            _authContext = sessionFactory.AuthContext;

        }

        /// <summary>
        ///  Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task SaveAuditDataAsync(List<AuditRequest> data, string moduleId, string entityTypeId, string entityId, int regionId = default(int))
        {
            CheckValidations(moduleId, entityTypeId);
            await _auditManager.SaveAuditDataAsync(data, moduleId, entityTypeId, entityId, _authContext.UserId, (regionId == default(int) ? _authContext.RegionId : regionId));
        }

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpPost("GroupCreate")]
        public async Task SaveAuditGroupDataAsync(List<AuditGroupRequest> data, string moduleId, string entityTypeId, string entityId, int regionId = default(int))
        {
            CheckValidations(moduleId, entityTypeId);
            await _auditManager.SaveAuditDataAsync(data, moduleId, entityTypeId, entityId, _authContext.UserId, (regionId == default(int) ? _authContext.RegionId : regionId));
        }

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        [HttpPost("GroupCreateByRegion")]
        public async Task SaveAuditGroupDataByRegionAsync(List<AuditGroupRequest> data, string moduleId, string entityTypeId, string entityId, int regionId)
        {
            CheckValidations(moduleId, entityTypeId);
            await _auditManager.SaveAuditDataAsync(data, moduleId, entityTypeId, entityId, _authContext.UserId, regionId);
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("Read")]
        public async Task<List<AuditResponse>> GetAuditDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber, int regionId = default(int))
        {
            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, (regionId == default(int) ? _authContext.RegionId : regionId));
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            return data;
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("ReadV1")]
        public async Task<List<AuditV1Response>> GetAuditDataV1Async(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber, int regionId = default(int))
        {
            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataV1Async(moduleId, entityTypeId, entityId, pageSize, pageNumber, (regionId == default(int) ? _authContext.RegionId : regionId));
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            return data;
        }

        /// <summary>
        ///     Gets all audit data results if no optional filters given or matching data based on given filters.
        /// </summary>
        /// <param name="moduleId">moduleId.</param>
        /// <param name="entityTypeId">entityTypeId.</param>
        /// <param name="entityId">entityId.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <param name="pageNumber">pageNumber.</param>
        /// <param name="fieldName">fieldName.</param>
        /// <param name="updatedBy">updatedBy.</param>
        /// <param name="updatedOn">updatedOn.</param>
        /// <returns>List of audit data results.</returns>
        [HttpGet("ReadSearch")]
        public async Task<List<AuditResponse>> GetAuditDataFieldSearchAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber,
            string fieldName = null, string updatedBy = null, DateTime? updatedOn = null)
        {
            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataSearchAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, _authContext.RegionId, fieldName, updatedBy, updatedOn);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            return data;
        }
        /// <summary>
        ///     Gets all audit data results if no optional filters given or matching data based on given filters.
        /// </summary>
        /// <param name="moduleId">moduleId.</param>
        /// <param name="entityTypeId">entityTypeId.</param>
        /// <param name="entityId">entityId.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <param name="pageNumber">pageNumber.</param>
        /// <param name="fieldName">fieldName.</param>
        /// <param name="updatedBy">updatedBy.</param>
        /// <param name="updatedOn">updatedOn.</param>
        /// <returns>List of audit data results.</returns>
        [HttpGet("ReadSearchV1")]
        public async Task<List<AuditV1Response>> GetAuditDataFieldSearchV1Async(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber,
            string fieldName = null, string updatedBy = null, DateTime? updatedOn = null)
        {
            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataSearchV1Async(moduleId, entityTypeId, entityId, pageSize, pageNumber, _authContext.RegionId, fieldName, updatedBy, updatedOn);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            return data;
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("GroupRead")]
        public async Task<List<AuditGroupResponse>> GetAuditGroupDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber)
        {

            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, _authContext.RegionId);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            var result = new List<AuditGroupResponse>();
            foreach (var item in data)
            {
                result.Add(new AuditGroupResponse()
                {
                    UserName = item.UserName,
                    Comment = item.Comment,
                    CreatedDate = item.CreatedDate,
                    OldValues = JsonConvert.DeserializeObject<List<string>>(item.OldValue),
                    NewValues = JsonConvert.DeserializeObject<List<string>>(item.NewValue),
                    AuditId = item.AuditId,
                    ParentAuditId = item.ParentAuditId
                });
            }

            return result;
        }


        /// <summary>
        ///  Get Multiple entities audit data.
        /// </summary>
        /// <param name="GetAuditDataRequestContract request"></param>
        /// <returns>list of AuditGroupResponse.</returns>
        [HttpPost("GroupRead")]
        public async Task<List<AuditGroupResponse>> GetAuditGroupDataAsync(GetAuditDataRequestContract request)
        {
            var data = await _auditManager.GetAuditDataAsync(request, _authContext.RegionId);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            var result = new List<AuditGroupResponse>();
            foreach (var item in data)
            {
                result.Add(new AuditGroupResponse()
                {
                    UserName = item.UserName,
                    Comment = item.Comment,
                    CreatedDate = item.CreatedDate,
                    OldValues = JsonConvert.DeserializeObject<List<string>>(item.OldValue),
                    NewValues = JsonConvert.DeserializeObject<List<string>>(item.NewValue),
                    AuditId = item.AuditId,
                    ParentAuditId = item.ParentAuditId
                });
            }

            return result;
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("GroupReadByRegion")]
        public async Task<List<AuditGroupResponse>> GetAuditGroupDataByRegionAsync(string moduleId, string entityTypeId, string entityId, int regionId, int pageSize, int pageNumber)
        {

            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, regionId);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            var result = new List<AuditGroupResponse>();
            foreach (var item in data)
            {
                result.Add(new AuditGroupResponse()
                {
                    UserName = item.UserName,
                    Comment = item.Comment,
                    CreatedDate = item.CreatedDate,
                    OldValues = JsonConvert.DeserializeObject<List<string>>(item.OldValue),
                    NewValues = JsonConvert.DeserializeObject<List<string>>(item.NewValue),
                    AuditId = item.AuditId,
                    ParentAuditId = item.ParentAuditId
                });
            }

            return result;
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("GroupReadV1")]
        public async Task<List<AuditV1GroupResponse>> GetAuditGroupDataV1Async(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber)
        {

            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, _authContext.RegionId);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            var result = new List<AuditGroupResponse>();
            foreach (var item in data)
            {
                result.Add(new AuditGroupResponse()
                {
                    UserName = item.UserName,
                    Comment = item.Comment,
                    CreatedDate = item.CreatedDate,
                    OldValues = JsonConvert.DeserializeObject<List<string>>(item.OldValue),
                    NewValues = JsonConvert.DeserializeObject<List<string>>(item.NewValue),
                    AuditId = item.AuditId,
                    ParentAuditId = item.ParentAuditId
                });
            }

            return result.ToAuditReponseTree();
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("GroupReadByRegionV1")]
        public async Task<List<AuditV1GroupResponse>> GetAuditGroupDataByRegionV1Async(string moduleId, string entityTypeId, string entityId, int regionId, int pageSize, int pageNumber)
        {

            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, regionId);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            var result = new List<AuditGroupResponse>();
            foreach (var item in data)
            {
                result.Add(new AuditGroupResponse()
                {
                    UserName = item.UserName,
                    Comment = item.Comment,
                    CreatedDate = item.CreatedDate,
                    OldValues = JsonConvert.DeserializeObject<List<string>>(item.OldValue),
                    NewValues = JsonConvert.DeserializeObject<List<string>>(item.NewValue),
                    AuditId = item.AuditId,
                    ParentAuditId = item.ParentAuditId
                });
            }

            return result.ToAuditReponseTree();
        }

        private void CheckValidations(string moduleId, string entityTypeId)
        {
            var validations = new List<ValidationInfo>();

            if (string.IsNullOrWhiteSpace(moduleId))
            {
                validations.Add(new ValidationInfo("ModuleId is required"));
            }

            if (string.IsNullOrWhiteSpace(entityTypeId))
            {
                validations.Add(new ValidationInfo("EntityTypeId is required"));
            }

            if (validations.Any())
            {
                throw new ValidationException("Missing required data", validations);
            }
        }
    }
}