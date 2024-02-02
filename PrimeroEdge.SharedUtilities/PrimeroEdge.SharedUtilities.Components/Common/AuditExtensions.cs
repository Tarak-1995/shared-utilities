using PrimeroEdge.SharedUtilities.Components.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components.Common
{
    public static class AuditExtensions
    {
        public static AuditLogEntity ToAuditTableStorage(this Audit audit)
        {
            return new AuditLogEntity
            {
                ModuleId = audit.ModuleId,
                AuditId = audit.AuditId,
                ParentAuditId = audit.ParentAuditId,
                Comment = audit.Comment,
                EntityId = audit.EntityId,
                EntityTypeId = audit.EntityTypeId,
                Field = audit.Field,
                NewValue = audit.NewValue,
                OldValue = audit.OldValue,
                RegionId = audit.RegionId,
            };
        }
        public static List<AuditLogEntity> ToAuditTableStorage(this List<Audit> auditEntries)
        {
            var auditLogEntityList = new List<AuditLogEntity>();
            foreach (var audit in auditEntries)
            {
                auditLogEntityList.Add(new AuditLogEntity
                {
                    ModuleId = audit.ModuleId,
                    AuditId = audit.AuditId,
                    ParentAuditId = audit.ParentAuditId,
                    Comment = audit.Comment,
                    EntityId = audit.EntityId,
                    EntityTypeId = audit.EntityTypeId,
                    Field = audit.Field,
                    NewValue = audit.NewValue,
                    OldValue = audit.OldValue,
                    RegionId = audit.RegionId,
                });
            }
            return auditLogEntityList;
        }        

        public static List<AuditV1Response> ToAuditReponseTree(this List<AuditResponse> flatList, Guid? parentAuditId = null)
        {
            return flatList
                .Where(node => node.ParentAuditId == parentAuditId)
                .Select(audit => new AuditV1Response
                {
                    AuditId = audit.AuditId,
                    ParentAuditId = audit.ParentAuditId,
                    Comment = audit.Comment,
                    Field = audit.Field,
                    NewValue = audit.NewValue,
                    OldValue = audit.OldValue,
                    Children = ToAuditReponseTree(flatList, audit.AuditId)
                }).ToList();
        }

        public static List<AuditV1GroupResponse> ToAuditReponseTree(this List<AuditGroupResponse> flatList, Guid? parentAuditId = null)
        {
            return flatList
                .Where(node => node.ParentAuditId == parentAuditId)
                .Select(audit => new AuditV1GroupResponse
                {
                    AuditId = audit.AuditId,
                    ParentAuditId = audit.ParentAuditId,
                    Comment = audit.Comment,
                    NewValues = audit.NewValues,
                    OldValues = audit.OldValues,
                    Children = ToAuditReponseTree(flatList, audit.AuditId)
                }).ToList();
        }

    }
}
