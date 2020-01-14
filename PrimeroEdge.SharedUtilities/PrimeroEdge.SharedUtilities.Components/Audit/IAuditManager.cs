/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// IAuditManager
    /// </summary>
    public interface IAuditManager
    {
        /// <summary>
        /// CreateAuditAsync
        /// </summary>
        /// <param name="audit"></param>
        /// <returns></returns>
        Task CreateAuditAsync(List<Audit> audit);

        /// <summary>
        /// GetAuditDataAsync
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        Task<List<Audit>> GetAuditDataAsync(EntityType entityTypeId, int entityId, string field);

    }
}
