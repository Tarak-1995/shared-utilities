/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cybersoft.Platform.Utilities.ResponseModels;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// AuditRepository
    /// </summary>
    public class AuditManager : SharedUtilitiesBase<AuditManager>, IAuditManager
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
        public async Task<List<Audit>> GetAuditDataAsync(AuditRequest request, int regionId)
        {
            var data = await _auditRepository.GetAuditDataAsync(request, regionId).ConfigureAwait(false);
            PaginationEnvelope = new Pagination(request.PageNumber, request.PageSize, (int)data.Item2);
            return data.Item1;
        }

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveAuditDataAsync(List<Audit> data, int userId, int regionId)
        {
            await _auditRepository.SaveAuditDataAsync(data, userId, regionId).ConfigureAwait(false);
        }
    }
}
