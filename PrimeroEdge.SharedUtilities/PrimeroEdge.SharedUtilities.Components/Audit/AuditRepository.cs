/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Cybersoft.Platform.Data.MongDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cybersoft.Platform.Message.Publisher;
using MongoDB.Driver;
using Newtonsoft.Json;

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

            var reqContext = GetTempRequestContextJson(); 
            var logReqContext = JsonConvert.DeserializeObject<IRequestContext>(reqContext);
            var payload = JsonConvert.SerializeObject(audit);
            var message = MessageBuilderHelper.BuildMessage(payload, MessageType.Audit, logReqContext);
            var type = _messagePublisher.Publisher.GetType();
            _messagePublisher.DispatchMessage(message);
            await Task.CompletedTask;
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


        private string GetTempRequestContextJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("'Tenant': {");
            sb.AppendLine("'Id': 'Tenant1',");
            sb.AppendLine("'IsdId': 0,");
            sb.AppendLine("'IsdName': null,");
            sb.AppendLine("'RealmType': 0,");
            sb.AppendLine("'RealmId': null,");
            sb.AppendLine("'DomainName': null,");
            sb.AppendLine("'AdminEmail': null,");
            sb.AppendLine("'CreatedOn': '2020-01-30T07:58:00',");
            sb.AppendLine("'CreatedBy': 'Demo User',");
            sb.AppendLine("'ModifiedOn': '2020-01-30T07:58:00',");
            sb.AppendLine("'ModifiedBy': 'Demo User',");
            sb.AppendLine("'IsActive': false,");
            sb.AppendLine("'Name': 'Tenant-X'");
            sb.AppendLine("},");
            sb.AppendLine("'User': {");
            sb.AppendLine("'UserId': 1,");
            sb.AppendLine("'UserName': 'Demo User',");
            sb.AppendLine("'FirstName': 'Demo User',");
            sb.AppendLine("'LastName': 'Demo User',");
            sb.AppendLine("'MI': null,");
            sb.AppendLine("'Email': 'Demo User@primeroedge.com'");
            sb.AppendLine("},");
            sb.AppendLine("'TenantSettings': {");
            sb.AppendLine("'Id': 1,");
            sb.AppendLine("'TenantId': 'Tenant1',");
            sb.AppendLine("'LoggerType': 'Serilog',");
            sb.AppendLine("'AuditProvider': null");
            sb.AppendLine("}");
            sb.AppendLine("}");
            return sb.ToString();
        }


    }
}
