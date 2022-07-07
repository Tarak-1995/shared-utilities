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
    using PrimeroEdge.SharedUtilities.Components.Common;

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
			if (updatedOn != null)
			{
                var updatedDate = (DateTime) updatedOn;
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

            if(!string.IsNullOrEmpty(updatedBy))
			    result = result.Where(s => !string.IsNullOrEmpty(s.UserName) && s.UserName.ToLower().Contains(updatedBy.ToLower())).ToList();

			return result;
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
            await this.SaveAuditDataAsync(new List<AuditRequest> {data}, moduleId, entityTypeId, entityId, userId, regionId);
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
            var request = new List<Audit>();
            moduleId = moduleId.Trim().ToUpper();
            entityId = entityId?.Trim().ToUpper();
            entityTypeId = entityTypeId.Trim().ToUpper();

            data.ForEach(x =>
            {
                request.Add(new Audit()
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
                    Comment = x.Comment
                });
            });

            await this._auditRepository.SaveAuditDataAsync(request);
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
            var request = new List<Audit>();
            moduleId = moduleId.Trim().ToUpper();
            entityId = entityId?.Trim().ToUpper();
            entityTypeId = entityTypeId.Trim().ToUpper();

            data.ForEach(x =>
            {
                request.Add(new Audit()
                {
                    AuditId = Guid.NewGuid(),
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow,
                    ModuleId = moduleId,
                    EntityTypeId = entityTypeId,
                    EntityId = !string.IsNullOrEmpty(x.EntityId)? x.EntityId.Trim().ToUpper() : entityId,
                    RegionId = regionId,
                    OldValue = JsonConvert.SerializeObject(x.OldValues ?? new List<string>()),
                    NewValue = JsonConvert.SerializeObject(x.NewValues ?? new List<string>()),
                    Comment = x.Comment
                });
            });

            await this._auditRepository.SaveAuditDataAsync(request);

        }
    }
}
