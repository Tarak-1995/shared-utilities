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
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task SaveAuditDataAsync(List<Audit> data)
        {
            var validations = new List<ValidationInfo>();

            if (_authContext == null || _authContext.RegionId <= 0)
            {
                validations.Add(new ValidationInfo("RegionId should be greater than zero."));
            }

            if (_authContext == null || _authContext.UserId <= 0)
            {
                validations.Add(new ValidationInfo("UserId should be greater than zero."));
            }

            if (validations.Any())
            {
                throw new ValidationException("Missing required data", validations);
            }

            await _auditManager.SaveAuditDataAsync(data, _authContext.UserId, _authContext.RegionId);
        }

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("GroupCreate")]
        public async Task SaveAuditGroupDataAsync(List<AuditGroup> data)
        {
            var validations = new List<ValidationInfo>();

            if (_authContext == null || _authContext.RegionId <= 0)
            {
                validations.Add(new ValidationInfo("RegionId should be greater than zero."));
            }

            if (_authContext == null || _authContext.UserId <= 0)
            {
                validations.Add(new ValidationInfo("UserId should be greater than zero."));
            }

            if (validations.Any())
            {
                throw new ValidationException("Missing required data", validations);
            }

            var req = new List<Audit>();

            foreach (var item in data)
            {
                req.Add(new Audit()
                {
                    EntityId = item.EntityId,
                    EntityTypeId = item.EntityTypeId,
                    UserName = item.UserName,
                    Comment = item.Comment,
                    OldValue = JsonConvert.SerializeObject(item.OldValues ?? new List<string>()),
                    NewValue = JsonConvert.SerializeObject(item.NewValues ?? new List<string>())
                });
            }

            await _auditManager.SaveAuditDataAsync(req, _authContext.UserId, _authContext.RegionId);
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Read")]
        public async Task<List<Audit>> GetAuditDataAsync(AuditRequest request)
        {
            var validations = new List<ValidationInfo>();
            if (_authContext == null || _authContext.RegionId <= 0)
            {
                validations.Add(new ValidationInfo("RegionId should be greater than zero."));
                throw new ValidationException("Missing required data", validations);
            }

            var data = await _auditManager.GetAuditDataAsync(request, _authContext.RegionId);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            return data;
        }


        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GroupRead")]
        public async Task<List<AuditGroup>> GetAuditGroupDataAsync(AuditRequest request)
        {
            var validations = new List<ValidationInfo>();
            if (_authContext == null || _authContext.RegionId <= 0)
            {
                validations.Add(new ValidationInfo("RegionId should be greater than zero."));
                throw new ValidationException("Missing required data", validations);
            }

            var data = await _auditManager.GetAuditDataAsync(request, _authContext.RegionId);

            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();

            var result = new List<AuditGroup>();
            foreach (var item in data)
            {
                result.Add(new AuditGroup()
                {
                    EntityId = item.EntityId,
                    EntityTypeId = item.EntityTypeId,
                    UserName = item.UserName,
                    Comment = item.Comment,
                    CreatedDate = item.CreatedDate,
                    OldValues = JsonConvert.DeserializeObject<List<string>>(item.OldValue),
                    NewValues = JsonConvert.DeserializeObject<List<string>>(item.NewValue)
                });
            }

            return result;
        }
    }
}