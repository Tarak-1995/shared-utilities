/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Cybersoft.Platform.Data.MongDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cybersoft.Platform.Utilities.Factories;
using Cybersoft.Platform.Utilities.ResponseModels;
using MongoDB.Driver;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// AuditRepository
    /// </summary>
    public class AuditRepository : IAuditRepository
    {

        /// <summary>
        /// mongoDbManager
        /// </summary>
        private readonly Lazy<Task<IMongoDbManager<Audit>>> _mongoDbManager;
        private readonly UserProfileModel _userProfile;



        /// <summary>
        /// AuditRepository
        /// </summary>
        /// <param name="mongoDbManager"></param>
        public AuditRepository(Lazy<Task<IMongoDbManager<Audit>>> mongoDbManager, IUserSessionFactory sessionFactory)
        {
            _mongoDbManager = mongoDbManager ?? throw new ArgumentNullException(nameof(mongoDbManager));
            _userProfile = sessionFactory.UserProfile;
        }

        /// <summary>
        /// Save audit data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveAuditDataAsync(List<Audit> data)
        {
            var mongoManager = await _mongoDbManager.Value.ConfigureAwait(false);
            data = data.Where(x => x.NewValue != x.OldValue).ToList();

            var utcNow = DateTime.UtcNow;
            data.ForEach(x =>
            {
                x.Id = Guid.NewGuid().ToString();
                x.CreatedDate = utcNow;
                x.UserId = _userProfile.UserId;
                x.RegionId = _userProfile.RegionId;
            });

            if(data.Any())
             await mongoManager.CreateAsync(data).ConfigureAwait(false);
        }

        /// <summary>
        /// Get audit data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Tuple<List<Audit>, long>> GetAuditDataAsync(AuditRequest request)
        {
            var mongoManager = await _mongoDbManager.Value.ConfigureAwait(false);

            var filter =  Builders<Audit>.Filter.Where(x => x.EntityTypeId == request.EntityTypeId && x.EntityId == request.EntityId && x.RegionId == _userProfile.RegionId);
            
            if(!string.IsNullOrWhiteSpace(request.Field))
               filter = filter & Builders<Audit>.Filter.Where(x=> x.Field.Equals(request.Field));

            var sort = Builders<Audit>.Sort.Descending(x => x.CreatedDate);

            if (request.PageNumber <= 0)
                request.PageNumber = 1;

            if (request.PageSize <= 0)
                request.PageSize = 100;

            var skip = (request.PageNumber - 1) * request.PageSize;

            var count = await mongoManager.CountDocumentsAsync(filter).ConfigureAwait(false);

            var data= await mongoManager.QueryAsync(filter, sort, skip, request.PageSize).ConfigureAwait(false);

            return Tuple.Create(data, count);

        }
    }
}
