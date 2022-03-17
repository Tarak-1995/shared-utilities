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
using Cybersoft.Platform.Data.Sql;
using Cybersoft.Platform.Couchbase.Client;
using System.Dynamic;
using System.Data;
using System.Data.SqlClient;
using Couchbase.Query;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// AuditRepository
    /// </summary>
    public class AuditRepository : IAuditRepository
    {

        private readonly ICouchbaseCluster _couchbaseCluster;
        private readonly ISqlDbManager _sqlDbManager;
        private const int CouchBatchSize = 500;
        private const string AuditBucket = "Audit";
        private const string TimeZoneCode = "DTTIMEZONE";
        private const string DayLightCode = "DAYLGTSATE";

        private const string GetTimeZoneSettings = @"SELECT 
                                                        UPPER(TRIM(S.SettingCode)) AS [SettingCode],                                                        
                                                        COALESCE(RSV.SettingValue, S.[SettingValue]) AS SettingValue
                                                    FROM NF_Setting S WITH (NOLOCK) 
                                                        LEFT JOIN NF_RegionSettingValue RSV WITH (NOLOCK) ON S.SettingId = RSV.SettingId AND RSV.RegionId = @RegionId
                                                    WHERE S.SettingCode IN ( 'DTTIMEZONE', 'DAYLGTSATE')";

        private const string GetUsers = @"SELECT UserId, CONCAT(TRIM(FirstName), ' ', TRIM(LastName)) AS [Name] FROM NF_User WITH(NOLOCK)
                                           WHERE UserId IN ({0})";

        private const string GetAuditPageData = @"SELECT V.* FROM Audit V
                                                WHERE V.type ='Audit' AND  V.regionId = {0} AND V.moduleId = '{1}' AND  V.entityTypeId = '{2}' AND   V.entityId = '{3}'         
                                                ORDER BY V.createdDate DESC
                                                OFFSET {4} LIMIT {5}";

        private const string GetAuditCountData = @"SELECT COUNT(*) AS Count FROM Audit V
                                                 WHERE V.type ='Audit' AND  V.regionId = {0} AND V.moduleId = '{1}' AND  V.entityTypeId = '{2}' AND   V.entityId = '{3}'";

		private const string GetAuditSearchCountQuery = @"SELECT COUNT(*) AS Count FROM Audit V
														 WHERE V.type = $type AND V.regionId = $regionId AND V.moduleId = $moduleId 
														 AND V.entityTypeId = $entityTypeId AND V.entityId = $entityId
														 AND (IS_NULL($field) OR CONTAINS (UPPER(V.field), $field)) 
	                                                     AND (IS_NULL($updatedOn) OR DATE_FORMAT_STR(V.createdDate, '1111-11-11') = $updatedOn)";

        private const string GetAuditSearchPageQuery = @"SELECT V.* FROM Audit V
			                                            WHERE V.type = $type AND V.regionId = $regionId  AND V.moduleId = $moduleId 
														AND V.entityTypeId = $entityTypeId AND V.entityId = $entityId 
														AND (IS_NULL($field) OR CONTAINS (UPPER(V.field), $field)) 
														AND (IS_NULL($updatedOn) OR DATE_FORMAT_STR(V.createdDate, '1111-11-11') = $updatedOn)
														ORDER BY V.createdDate DESC
			                                            OFFSET $offset LIMIT $limit";

        /// <summary>
        /// AuditRepository
        /// </summary>
        /// <param name="couchbaseCluster"></param>
        /// <param name="sqlDbManager"></param>
        public AuditRepository(ICouchbaseCluster couchbaseCluster, ISqlDbManager sqlDbManager)
        {
            this._couchbaseCluster = couchbaseCluster;
            this._sqlDbManager = sqlDbManager;
        }

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveAuditDataAsync(List<Audit> data)
        {
            data = data.Where(x => x.NewValue != x.OldValue).ToList();
            if (data.Any())
            {
                await this._couchbaseCluster.UpdateAsync(AuditBucket, data, CouchBatchSize);
            }
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
        public async Task<Tuple<List<Audit>, int>> GetAuditDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, int pageNumber, int regionId)
        {
            if (pageNumber <= 0)
                pageNumber = 1;

            if (pageSize <= 0)
                pageSize = 20;

            var offset = (pageNumber - 1) * pageSize;
            var limit = pageSize;

            var count = 0;
            var pageData = new List<Audit>();

            var query = string.Format(GetAuditCountData, regionId, moduleId.Trim().ToUpper(), entityTypeId.Trim().ToUpper(), entityId.Trim().ToUpper());
            var result = await this._couchbaseCluster.QueryAsync<ExpandoObject>(query);
            await foreach (dynamic item in result)
            {
                count = Convert.ToInt32(item.Count);
            }

            if (count != 0)
            {
                query = string.Format(GetAuditPageData, regionId, moduleId.Trim().ToUpper(), entityTypeId.Trim().ToUpper(), entityId.Trim().ToUpper(), offset, limit);
                var data = await this._couchbaseCluster.QueryAsync<Audit>(query);
                pageData = await data.ToListAsync();
            }

            return Tuple.Create(pageData, count);

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
        /// <param name="updatedOn">updatedOn.</param>
        /// <returns></returns>
        public async Task<Tuple<List<Audit>, int>> GetAuditSearchDataAsync(string moduleId, string entityTypeId, string entityId, int pageSize, 
            int pageNumber, int regionId, string fieldName, string updatedOn)
        {
	        if (pageNumber <= 0)
		        pageNumber = 1;

	        if (pageSize <= 0)
		        pageSize = 20;

	        var offset = (pageNumber - 1) * pageSize;
	        var limit = pageSize;

	        var count = 0;
	        var pageData = new List<Audit>();

            var auditSearchCountData = await GetAuditSearchCountData(moduleId.Trim().ToUpper(), entityTypeId.Trim().ToUpper(), entityId.Trim().ToUpper(), regionId, fieldName?.Trim().ToUpper(), updatedOn);

			await foreach (dynamic item in auditSearchCountData)
	        {
		        count = Convert.ToInt32(item.Count);
	        }

	        if (count != 0)
	        {
                var auditSearchPageData = await GetAuditSearchPageResult(moduleId.Trim().ToUpper(), entityTypeId.Trim().ToUpper(), entityId.Trim().ToUpper(), regionId, fieldName?.Trim().ToUpper(), updatedOn, offset, limit);
                pageData = await auditSearchPageData.ToListAsync();
            }

	        return Tuple.Create(pageData, count);
        }

        /// <summary>
        /// GetUsersAsync
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<Dictionary<int, string>> GetUsersAsync(List<int> users)
        {
            var query = string.Format(GetUsers, string.Join(',', users.Distinct()));
            var table = await this._sqlDbManager.GetDataAsync(query, null, CommandType.Text);

            return table.AsEnumerable()
                .GroupBy(x => x.Field<int>(0))
                .ToDictionary(
                    row => row.Key,
                    row => row.First().Field<string>(1));
        }

        /// <summary>
        /// GetTimeZoneSettingsAsync
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public async Task<Tuple<string, bool>> GetTimeZoneSettingsAsync(int regionId)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@RegionId", regionId),
            };

            var table = await this._sqlDbManager.GetDataAsync(GetTimeZoneSettings, sqlParameters, CommandType.Text);
            var result = table.AsEnumerable()
                .GroupBy(x => x.Field<string>(0))
                .ToDictionary(
                    row => row.Key,
                    row => row.First().Field<string>(1));

            var timeZone = result[TimeZoneCode];
            var isDayLight = result[DayLightCode];
            return Tuple.Create(timeZone, isDayLight == "1");
        }

        private async Task<IQueryResult<ExpandoObject>> GetAuditSearchCountData(string moduleId, string entityTypeId, string entityId, int regionId, string fieldName, string updatedOn)
        {

            var countData = await _couchbaseCluster.QueryAsync<ExpandoObject>(GetAuditSearchCountQuery, parameters =>
            {
                parameters.Parameter("type", nameof(Audit));
                parameters.Parameter("moduleId", moduleId);
                parameters.Parameter("entityTypeId", entityTypeId);
                parameters.Parameter("entityId", entityId);
                parameters.Parameter("regionId", regionId);
                parameters.Parameter("field", fieldName);
                parameters.Parameter("updatedOn", updatedOn);
            });

            return countData;
        }

        private async Task<IQueryResult<Audit>> GetAuditSearchPageResult(string moduleId, string entityTypeId, string entityId, int regionId, string fieldName, string updatedOn, int offset, int limit)
        {

            var pageData = await _couchbaseCluster.QueryAsync<Audit>(GetAuditSearchPageQuery, parameters =>
            {
                parameters.Parameter("type", nameof(Audit));
                parameters.Parameter("moduleId", moduleId);
                parameters.Parameter("entityTypeId", entityTypeId);
                parameters.Parameter("entityId", entityId);
                parameters.Parameter("regionId", regionId);
                parameters.Parameter("field", fieldName);
                parameters.Parameter("updatedOn", updatedOn);
                parameters.Parameter("offset", offset);
                parameters.Parameter("limit", limit);
            });

            return pageData;
        }
    }
}
