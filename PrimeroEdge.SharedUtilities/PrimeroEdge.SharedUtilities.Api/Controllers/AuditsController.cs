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
        /// Get audits data
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        [HttpGet("entityType/{entityTypeId}/entity/{entityId}")]
        public async Task<List<Audit>> GetAuditDataAsync(int entityTypeId, int entityId, string field)
        {
            return  await _auditManager.GetAuditDataAsync(entityTypeId, entityId, field);
        }

        /// <summary>
        /// Create audits
        /// </summary>
        /// <param name="audit"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAuditAsync(List<Audit> audit)
        {
            await _auditManager.CreateAuditAsync(audit);
        }

    }
}