/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cybersoft.Platform.Message.Publisher;
using Newtonsoft.Json;
using System.Text;

namespace PrimeroEdge.SharedUtilities.Api.Controllers
{

    /// <summary>
    /// MessagePublisherController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessagePublisherController : ControllerBase
    {
        /// <summary>
        /// MessagePublisher
        /// </summary>
        private readonly MessagePublisher _messagePublisher;

        /// <summary>
        /// MessagePublisherController
        /// </summary>
        /// <param name="messagePublisher"></param>
        public MessagePublisherController(MessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }


        /// <summary>
        /// Publish messages
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        [HttpPost("{messageType}")]
        public async Task PublishMessageAsync(MessageType messageType)
        {
            var payload = await GetRequestBodyAsync();
            if (!string.IsNullOrWhiteSpace(payload))
            {
                var logReqContext = JsonConvert.DeserializeObject<IRequestContext>(GetTempRequestContextJson());
                var data = MessageBuilderHelper.BuildMessage(payload, messageType, logReqContext);
                _messagePublisher.DispatchMessage(data);
            }
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

        /// <summary>
        /// GetRequestBodyAsync
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetRequestBodyAsync()
        {
            using var reader = new StreamReader(Request.Body);
            return  await reader.ReadToEndAsync();
        }
    }
}