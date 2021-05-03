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
        public async Task SaveAuditDataAsync(List<Audit> data, string moduleId, string entityTypeId, string entityId)
        {

            CheckValidations(moduleId, entityTypeId);

            data.ForEach(x =>
            {
                x.Id = Guid.NewGuid().ToString();
                x.CreatedDate = DateTime.UtcNow;
                x.UserId = _authContext.UserId;
                x.RegionId = _authContext.RegionId;
                x.ModuleId = moduleId?.Trim().ToUpper();
                x.EntityTypeId = entityTypeId?.Trim().ToUpper();
                x.EntityId = entityId?.Trim().ToUpper();
            });

            await _auditManager.SaveAuditDataAsync(data);
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
        public async Task SaveAuditGroupDataAsync(List<AuditGroup> data, string moduleId, string entityTypeId, string entityId)
        {
            CheckValidations(moduleId, entityTypeId);
            var req = new List<Audit>();

            foreach (var item in data)
            {
                req.Add(new Audit()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.UtcNow,
                    UserId = _authContext.UserId,
                    RegionId = _authContext.RegionId,
                    EntityId = entityId?.Trim().ToUpper(),
                    EntityTypeId = entityTypeId?.Trim().ToUpper(),
                    ModuleId = moduleId?.Trim().ToUpper(),
                    UserName = item.UserName,
                    Comment = item.Comment,
                    OldValue = JsonConvert.SerializeObject(item.OldValues ?? new List<string>()),
                    NewValue = JsonConvert.SerializeObject(item.NewValues ?? new List<string>())
                });
            }

            await _auditManager.SaveAuditDataAsync(req);
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
        public async Task<List<Audit>> GetAuditDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber)
        {
            var request = new AuditRequest
            {
                ModuleId = moduleId,
                EntityTypeId = entityTypeId,
                EntityId = entityId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            CheckValidations(request.ModuleId, request.EntityTypeId);

            var data = await _auditManager.GetAuditDataAsync(request, _authContext.RegionId);
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
        public async Task<List<AuditGroup>> GetAuditGroupDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber)
        {
            var request = new AuditRequest
            {
                ModuleId = moduleId,
                EntityTypeId = entityTypeId,
                EntityId = entityId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            CheckValidations(request.ModuleId, request.EntityTypeId);

            var data = await _auditManager.GetAuditDataAsync(request, _authContext.RegionId);

            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();

            var result = new List<AuditGroup>();
            foreach (var item in data)
            {
                result.Add(new AuditGroup()
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