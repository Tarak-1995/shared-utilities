/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// AuditRepository
    /// </summary>
    public class AuditManager : IAuditManager
    {

        /// <summary>
        /// auditRepository
        /// </summary>
        private readonly IAuditRepository _auditRepository;

        /// <summary>
        /// auditRepository
        /// </summary>
        /// <param name="auditRepository"></param>
        public AuditManager(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository ?? throw new ArgumentNullException(nameof(auditRepository));
        }
        

        /// <summary>
        /// Get Audit Data
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<List<Audit>> GetAuditDataAsync(int entityTypeId, int entityId, string field)
        {
            return await _auditRepository.GetAuditDataAsync(entityTypeId, entityId, field).ConfigureAwait(false);
        }
    }
}
