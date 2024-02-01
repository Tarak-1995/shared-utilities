/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */
using Cybersoft.Platform.Couchbase.Entities;
using System;
using System.Collections.Generic;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// Audit
    /// </summary>
    public class Audit : BaseEntity
    {
        /// <summary>
        /// Gets key
        /// </summary>
        public override string Id => $"{this.Type}_{this.AuditId.ToString().ToLower()}";

        /// <summary>
        /// Gets or sets verificationSampleId
        /// </summary>
        public Guid AuditId { get; set; }

        /// <summary>
        /// Gets totalAppCount
        /// </summary>
        public override string Type => nameof(Audit);

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
    }

    

   
}
