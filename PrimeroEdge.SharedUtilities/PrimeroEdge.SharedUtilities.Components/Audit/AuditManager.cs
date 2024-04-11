/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cybersoft.Platform.Utilities.ResponseModels;
using Newtonsoft.Json;

namespace PrimeroEdge.SharedUtilities.Components
{
    using Cybersoft.Platform.DocumentStorage.AzureTable;
    using PrimeroEdge.SharedUtilities.Components.Common;
    using PrimeroEdge.SharedUtilities.Components.Models;
    using TableStorage.Abstractions.Store;

    /// <summary>
    /// AuditRepository
    /// </summary>
    public class AuditManager : SharedUtilitiesBase<AuditManager>, IAuditManager
    {

        /// <summary>
        /// auditRepository
        /// </summary>
        private readonly IAuditRepository _auditRepository;
        private readonly IAzureTableStorage<AuditLogEntity> _azureTableService;

        /// <summary>
        /// auditRepository
        /// </summary>
        /// <param name="auditRepository"></param>
        public AuditManager(IAuditRepository auditRepository, IAzureTableStorage<AuditLogEntity> azureTableService)
        {
            _auditRepository = auditRepository ?? throw new ArgumentNullException(nameof(auditRepository));
            _azureTableService = azureTableService ?? throw new ArgumentNullException(nameof(azureTableService));
        }


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
        public async Task<List<AuditResponse>> GetAuditDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber, int regionId)
        {
            var data = await _auditRepository.GetAuditDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, regionId);

            var result = new List<AuditResponse>();
            if (data.Item2 != 0)
            {
                var settings = await this._auditRepository.GetTimeZoneSettingsAsync(regionId);
                var userIdList = data.Item1.Select(x => x.CreatedBy).Distinct().ToList();
                var users = await this._auditRepository.GetUsersAsync(userIdList);

                foreach (var item in data.Item1)
                {
                    var row = new AuditResponse()
                    {
                        CreatedDate = this.GetDistrictDateTime(item.CreatedDate, settings.Item1, settings.Item2),
                        Field = item.Field,
                        OldValue = item.OldValue,
                        NewValue = item.NewValue,
                        Comment = item.Comment,
                        UserName = users.ContainsKey(item.CreatedBy) ? users[item.CreatedBy] : null,
                        AuditId = item.AuditId,
                        ParentAuditId = item.ParentAuditId,
                    };
                    result.Add(row);
                }
            }

            PaginationEnvelope = new Pagination(pageNumber, pageSize, data.Item2);
            return result;
        }

        /// <summary>
        /// Gets audit data based on matching field search.
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
        public async Task<List<AuditResponse>> GetAuditDataSearchAsync(string moduleId, string entityTypeId, string entityId, int pageSize,
            int pageNumber, int regionId, string fieldName, string updatedBy, DateTime? updatedOn)
        {
            var settings = await this._auditRepository.GetTimeZoneSettingsAsync(regionId);
            string fromDate = null;
            string toDate = null;
            if (updatedOn.HasValue)
            {
                var updatedDate = (DateTime)updatedOn;
                fromDate = updatedDate.ToUtcDateTime(false, settings.Item1);
                toDate = updatedDate.ToUtcDateTime(true, settings.Item1);
            }

            var data = await _auditRepository.GetAuditSearchDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber,
                regionId, fieldName, fromDate, toDate);

            var result = new List<AuditResponse>();
            if (data.Item2 != 0)
            {
                var userIdList = data.Item1.Select(x => x.CreatedBy).Distinct().ToList();
                var users = await this._auditRepository.GetUsersAsync(userIdList);

                foreach (var item in data.Item1)
                {
                    var row = new AuditResponse()
                    {
                        CreatedDate = this.GetDistrictDateTime(item.CreatedDate, settings.Item1, settings.Item2),
                        Field = item.Field,
                        OldValue = item.OldValue,
                        NewValue = item.NewValue,
                        Comment = item.Comment,
                        UserName = users.ContainsKey(item.CreatedBy) ? users[item.CreatedBy] : null,
                    };
                    result.Add(row);
                }
            }

            PaginationEnvelope = new Pagination(pageNumber, pageSize, data.Item2);

            if (!string.IsNullOrEmpty(updatedBy))
                result = result.Where(s => !string.IsNullOrEmpty(s.UserName) && s.UserName.ToLower().Contains(updatedBy.ToLower())).ToList();

            return result;
        }


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
        public async Task<List<AuditV1Response>> GetAuditDataV1Async(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber, int regionId)
        {
            var result = await GetAuditDataAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, regionId);
            return result.ToAuditReponseTree();
        }

        /// <summary>
        /// Gets audit data based on matching field search.
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
        public async Task<List<AuditV1Response>> GetAuditDataSearchV1Async(string moduleId, string entityTypeId, string entityId, int pageSize,
            int pageNumber, int regionId, string fieldName, string updatedBy, DateTime? updatedOn)
        {
            var result = await GetAuditDataSearchAsync(moduleId, entityTypeId, entityId, pageSize, pageNumber, regionId, fieldName, updatedBy, updatedOn);
            return result.ToAuditReponseTree();
        }

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
        public async Task SaveAuditDataAsync(AuditRequest data, string moduleId, string entityTypeId, string entityId, int userId, int regionId)
        {
            await this.SaveAuditDataAsync(new List<AuditRequest> { data }, moduleId, entityTypeId, entityId, userId, regionId);
        }

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
        public async Task SaveAuditDataAsync(List<AuditRequest> data, string moduleId, string entityTypeId, string entityId, int userId, int regionId)
        {
            var request = GetSummarizedAuditRequest(data, moduleId, entityTypeId, entityId, userId, regionId);
            await this._auditRepository.SaveAuditDataAsync(request);
            await _azureTableService.Insert(request.ToAuditTableStorage());
        }

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
        public async Task SaveAuditDataAsync(List<AuditGroupRequest> data, string moduleId, string entityTypeId, string entityId, int userId, int regionId)
        {
            var request = GetSummarizedAuditGroupRequest(data, moduleId, entityTypeId, entityId, userId, regionId);
            await this._auditRepository.SaveAuditDataAsync(request);
            await _azureTableService.Insert(request.ToAuditTableStorage());
        }


        private List<Audit> GetSummarizedAuditRequest(List<AuditRequest> data, string moduleId, string entityTypeId, string entityId, int userId, int regionId, Guid? parentAuditId = null)
        {
            var request = new List<Audit>();
            moduleId = moduleId.Trim().ToUpper();
            entityId = entityId?.Trim().ToUpper();
            entityTypeId = entityTypeId.Trim().ToUpper();

            foreach (var x in data)
            {
                var auditEntry = new Audit()
                {
                    AuditId = Guid.NewGuid(),
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow,
                    ModuleId = moduleId,
                    EntityTypeId = entityTypeId,
                    EntityId = !string.IsNullOrEmpty(x.EntityId) ? x.EntityId.Trim().ToUpper() : entityId,
                    RegionId = regionId,
                    OldValue = x.OldValue,
                    NewValue = x.NewValue,
                    Field = x.Field,
                    Comment = x.Comment,
                    ParentAuditId = parentAuditId
                };

                request.Add(auditEntry);

                // Recursive call for nested data
                if (x.childAuditRequest != null && x.childAuditRequest.Any())
                {
                    request.AddRange(GetSummarizedAuditRequest(x.childAuditRequest, moduleId, entityTypeId, entityId, userId, regionId, auditEntry.AuditId));
                }
            }

            return request;
        }

        private List<Audit> GetSummarizedAuditGroupRequest(List<AuditGroupRequest> data, string moduleId, string entityTypeId, string entityId, int userId, int regionId, Guid? parentAuditId = null)
        {
            var request = new List<Audit>();
            moduleId = moduleId.Trim().ToUpper();
            entityId = entityId?.Trim().ToUpper();
            entityTypeId = entityTypeId.Trim().ToUpper();

            foreach (var x in data)
            {
                var auditEntry = new Audit()
                {
                    AuditId = Guid.NewGuid(),
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow,
                    ModuleId = moduleId,
                    EntityTypeId = entityTypeId,
                    EntityId = !string.IsNullOrEmpty(x.EntityId) ? x.EntityId.Trim().ToUpper() : entityId,
                    RegionId = regionId,
                    OldValue = JsonConvert.SerializeObject(x.OldValues ?? new List<string>()),
                    NewValue = JsonConvert.SerializeObject(x.NewValues ?? new List<string>()),
                    Field = x.Field,
                    Comment = x.Comment,
                    ParentAuditId = parentAuditId
                };

                request.Add(auditEntry);

                // Recursive call for nested data
                if (x.childAuditRequest != null && x.childAuditRequest.Any())
                {
                    request.AddRange(GetSummarizedAuditGroupRequest(x.childAuditRequest, moduleId, entityTypeId, entityId, userId, regionId, auditEntry.AuditId));
                }
            }

            return request;
        }

    }
}
