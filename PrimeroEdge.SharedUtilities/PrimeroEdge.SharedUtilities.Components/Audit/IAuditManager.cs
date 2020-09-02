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
        /// Get audit data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<Audit>> GetAuditDataAsync(AuditRequest request);


        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SaveAuditDataAsync(List<Audit> data);

    }
}
