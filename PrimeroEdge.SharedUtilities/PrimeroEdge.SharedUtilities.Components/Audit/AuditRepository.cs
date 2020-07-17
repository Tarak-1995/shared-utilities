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
using Cybersoft.Platform.Message.Publisher;
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

        /// <summary>
        /// MessagePublisher
        /// </summary>
        private readonly MessagePublisher _messagePublisher;

        /// <summary>
        /// AuditRepository
        /// </summary>
        /// <param name="mongoDbManager"></param>
        /// <param name="messagePublisher"></param>
        public AuditRepository(Lazy<Task<IMongoDbManager<Audit>>> mongoDbManager, MessagePublisher messagePublisher)
        {
            _mongoDbManager = mongoDbManager ?? throw new ArgumentNullException(nameof(mongoDbManager));
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        }

        /// <summary>
        /// Get Audit Data
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<List<Audit>> GetAuditDataAsync(int entityTypeId, int entityId, string field)
        {
            var mongoDbManager = await _mongoDbManager.Value.ConfigureAwait(false);

            var filter = string.IsNullOrWhiteSpace(field)
                ? Builders<Audit>.Filter.Where(x => x.EntityTypeId == entityTypeId && x.EntityId == entityId)
                : Builders<Audit>.Filter.Where(x =>
                    x.EntityTypeId == entityTypeId && x.EntityId == entityId && x.Field.Equals(field));

            return await mongoDbManager.QueryAsync(filter).ConfigureAwait(false);

        }
    }
}
