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
        public async Task SaveAuditDataAsync(List<AuditRequest> data, string moduleId, string entityTypeId, string entityId)
        {
            CheckValidations(moduleId, entityTypeId);
            await _auditManager.SaveAuditDataAsync(data, moduleId, entityTypeId, entityId, _authContext.UserId, _authContext.RegionId);
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
        public async Task SaveAuditGroupDataAsync(List<AuditGroupRequest> data, string moduleId, string entityTypeId, string entityId)
        {
            CheckValidations(moduleId, entityTypeId);
            await _auditManager.SaveAuditDataAsync(data, moduleId, entityTypeId, entityId, _authContext.UserId, _authContext.RegionId);
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
        public async Task<List<AuditResponse>> GetAuditDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber)
        {
            CheckValidations(moduleId, entityTypeId);
            var data = await _auditManager.GetAuditDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, 33);
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
                    NewValues = JsonConvert.DeserializeObject<List<string>>(item.NewValue)
                });
            }

            return result;
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