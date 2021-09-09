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
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task<List<AuditResponse>> GetAuditDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber, int regionId);

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="userId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task SaveAuditDataAsync(AuditRequest data, string moduleId, string entityTypeId, string entityId, int userId, int regionId);


        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="userId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task SaveAuditDataAsync(List<AuditRequest> data, string moduleId, string entityTypeId, string entityId, int userId, int regionId);

      /// <summary>
      /// Save audit data
      /// </summary>
      /// <param name="data"></param>
      /// <param name="moduleId"></param>
      /// <param name="entityTypeId"></param>
      /// <param name="entityId"></param>
      /// <param name="userId"></param>
      /// <param name="regionId"></param>
      /// <returns></returns>
        Task SaveAuditDataAsync(List<AuditGroupRequest> data, string moduleId, string entityTypeId, string entityId, int userId, int regionId);

    }
}
