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
using System.Threading.Tasks;

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

        /// <summary>
        /// AuditsController
        /// </summary>
        /// <param name="auditManager"></param>
        public AuditsController(IAuditManager auditManager)
        {
            _auditManager = auditManager ?? throw new ArgumentNullException(nameof(auditManager));
        }

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task SaveAuditDataAsync(List<Audit> data)
        {
            await _auditManager.SaveAuditDataAsync(data);
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Read")]
        public async Task<List<Audit>> GetAuditDataAsync(AuditRequest request)
        {
            return  await _auditManager.GetAuditDataAsync(request);
        }
    }
}