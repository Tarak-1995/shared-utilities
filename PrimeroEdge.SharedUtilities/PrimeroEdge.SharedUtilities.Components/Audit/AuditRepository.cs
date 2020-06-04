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
        private readonly IMongoDbManager<Audit> _mongoDbManager;

        /// <summary>
        /// MessagePublisher
        /// </summary>
        private MessagePublisher _messagePublisher;

        /// <summary>
        /// AuditRepository
        /// </summary>
        /// <param name="mongoDbManager"></param>
        public AuditRepository(IMongoDbManager<Audit> mongoDbManager, MessagePublisher messagePublisher)
        {
            if (mongoDbManager == null)
                throw new ArgumentNullException(nameof(mongoDbManager));

            _mongoDbManager = mongoDbManager;

            _messagePublisher = messagePublisher;
        }

        /// <summary>
        /// Temporary Request Context generation - ToDO: Should be taken from API context
        /// </summary>
        /// <returns></returns>
        private string GetTempRequestContextJson()
        {
            StringBuilder sb = new StringBuilder();
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
            sb.AppendLine("'CreatedBy': 'Srini',");
            sb.AppendLine("'ModifiedOn': '2020-01-30T07:58:00',");
            sb.AppendLine("'ModifiedBy': 'Srini',");
            sb.AppendLine("'IsActive': false,");
            sb.AppendLine("'Name': 'Tenant-X'");
            sb.AppendLine("},");
            sb.AppendLine("'User': {");
            sb.AppendLine("'UserId': 1,");
            sb.AppendLine("'UserName': 'srinip',");
            sb.AppendLine("'FirstName': 'Srinivasarao',");
            sb.AppendLine("'LastName': 'Pepakayala',");
            sb.AppendLine("'MI': null,");
            sb.AppendLine("'Email': 'Srinivasarao.pepakayala@primeroedge.com'");
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

            //Temp Code for Contracts to Publisher Contracts conversion
            var reqContext = GetTempRequestContextJson(); //ToDo: Temp - It should be from request context.
            //Temp Code for Contracts to Publisher Contracts conversion
            var logReqContext = JsonConvert.DeserializeObject<IRequestContext>(reqContext);

            var payload = JsonConvert.SerializeObject(audit);
            //API has to construct this message and send the command to Framework. Messagebuiler build command code will move to API side.
            IMessage message = MessageBuilderHelper.BuildMessage(payload, MessageType.Audit, logReqContext);
            var type = _messagePublisher.Publisher.GetType();
            //message.Identifier = SessionId:: LoggedIn User Sesstion ID - TBD;
            _messagePublisher.DispatchMessage(message);


            //await _mongoDbManager.CreateAsync(audit).ConfigureAwait(false);

            await Task.CompletedTask;
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
