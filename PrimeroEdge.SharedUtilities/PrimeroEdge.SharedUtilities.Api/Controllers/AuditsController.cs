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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Cybersoft.Platform.Authorization.HeaderUtilities.Models;
using Cybersoft.Platform.Authorization.HeaderUtilities.Factories;
using Cybersoft.Platform.Utilities.Exceptions;

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
                validations.Add(new ValidationInfo("RegionId should be greater han zero."));
                throw new ValidationException("Missing required data", validations);
            }

            var data = await _auditManager.GetAuditDataAsync(request, _authContext.RegionId);
            HttpContext.Items[APIConstants.RESPONSE_PAGINATION] = _auditManager.GetPaginationEnvelope();
            return data;
        }
    }
}