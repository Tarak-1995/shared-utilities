using Azure;
using Azure.Data.Tables;
using System;


namespace PrimeroEdge.SharedUtilities.Components.Models
{
    public class AuditLogEntity : ITableEntity
    {
        public AuditLogEntity()
        {            
        }
        /// <summary>
        /// Gets or sets verificationSampleId
        /// </summary>
        public Guid AuditId { get; set; }

        /// <summary>
        /// Get or set RegionId
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Get or set ModuleId
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// Get or set EntityTypeId
        /// </summary>
        public string EntityTypeId { get; set; }

        /// <summary>
        /// Get or set EntityId
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Get or set Field
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Get or set OldValue
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Get or set NewValue
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Get or set Comment
        /// </summary>
        public string Comment { get; set; }

        public Guid? ParentAuditId { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
