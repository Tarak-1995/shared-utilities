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
        /// Get audit data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<Audit>> GetAuditDataAsync(AuditRequest request)
        {
            return await _auditRepository.GetAuditDataAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveAuditDataAsync(List<Audit> data)
        {
            await _auditRepository.SaveAuditDataAsync(data).ConfigureAwait(false);
        }
    }
}
