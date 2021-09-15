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
    /// IAuditRepository
    /// </summary>
    public interface IAuditRepository
    {
        /// <summary>
        ///  Get audit data
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task<Tuple<List<Audit>, int>> GetAuditDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber, int regionId);


        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SaveAuditDataAsync(List<Audit> data);

        /// <summary>
        /// GetUsersAsync
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        Task<Dictionary<int, string>> GetUsersAsync(List<int> users);

        /// <summary>
        /// GetTimeZoneSettingsAsync
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task<Tuple<string, bool>> GetTimeZoneSettingsAsync(int regionId);

    }
}
