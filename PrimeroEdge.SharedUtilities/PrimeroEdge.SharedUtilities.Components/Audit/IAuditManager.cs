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
        ///  Gets audit data based on matching field search.
        /// </summary>
        /// <param name="moduleId">moduleId.</param>
        /// <param name="entityTypeId">entityTypeId.</param>
        /// <param name="entityId">entityId.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <param name="pageNumber">pageNumber.</param>
        /// <param name="regionId">regionId.</param>
        /// <param name="fieldName">fieldName.</param>
        /// <param name="updatedBy">updatedBy.</param>
        /// <param name="updatedOn">updatedOn.</param>
        /// <returns></returns>
        Task<List<AuditResponse>> GetAuditDataSearchAsync(string moduleId, string entityTypeId, string entityId, 
	        int pageSize, int pageNumber, int regionId, string fieldName, string updatedBy, DateTime? updatedOn);

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
        Task SaveAuditDataAsync(List<AuditRequest> data, string moduleId, string entityTypeId, string entityId, int userId, int regionId, Guid? parentAuditId=null);

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
        Task SaveAuditDataAsync(List<AuditGroupRequest> data, string moduleId, string entityTypeId, string entityId, int userId, int regionId, Guid? parentAuditId = null);

    }
}
