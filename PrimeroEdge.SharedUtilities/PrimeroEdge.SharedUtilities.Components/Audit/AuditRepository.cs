/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Cybersoft.Platform.Data.MongDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly IMongoDbManager<Audit> _mongoDbManager;

        /// <summary>
        /// AuditRepository
        /// </summary>
        /// <param name="mongoDbManager"></param>
        public AuditRepository(IMongoDbManager<Audit> mongoDbManager)
        {
            if (mongoDbManager == null)
                throw new ArgumentNullException(nameof(mongoDbManager));

            _mongoDbManager = mongoDbManager;
        }


        /// <summary>
        /// CreateAuditAsync
        /// </summary>
        /// <param name="audit"></param>
        /// <returns></returns>
        public async Task CreateAuditAsync(List<Audit> audit)
        {
            audit.ForEach(x => 
            {
                x.Id = Guid.NewGuid().ToString();
                x.CreatedDate = DateTime.Now;
            });
            await _mongoDbManager.CreateAsync(audit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Audit Data
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<List<Audit>> GetAuditDataAsync(EntityType entityTypeId, int entityId, string field)
        {
            if (string.IsNullOrWhiteSpace(field))
                return await _mongoDbManager.QueryAsync(x => x.EntityTypeId == entityTypeId && x.EntityId == entityId).ConfigureAwait(false);
            else
                return await _mongoDbManager.QueryAsync(x => x.EntityTypeId == entityTypeId && x.EntityId == entityId && x.Field.Equals(field)).ConfigureAwait(false);
        }
    }
}
