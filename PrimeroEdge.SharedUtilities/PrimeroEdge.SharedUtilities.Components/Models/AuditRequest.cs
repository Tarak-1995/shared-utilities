/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Collections.Generic;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// AuditResponse
    /// </summary>
    public class AuditRequest
    {
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

        /// <summary>
        /// Get or set EntityId
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Get or set ChildAuditRequest.
        /// </summary>
        public List<AuditRequest> childAuditRequest { get; set; }

    }


    public class AuditGroupRequest
    {
        /// <summary>
        /// Get or set Field
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Get or set OldValue
        /// </summary>
        public List<string> OldValues { get; set; }


        /// <summary>
        /// Get or set NewValues
        /// </summary>
        public List<string> NewValues { get; set; }

        /// <summary>
        /// Get or set Comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Get or set EntityId
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Get or set ChildAuditRequest.
        /// </summary>
        public List<AuditGroupRequest> childAuditRequest { get; set; }

    }
}
