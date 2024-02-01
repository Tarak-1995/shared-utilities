/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// AuditResponse
    /// </summary>
    public class AuditResponse
    {
        /// <summary>
        /// Get or set CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }

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
        /// Get or set userName
        /// </summary>
        public string UserName { get; set; }

        public Guid AuditId { get; set; }
        public Guid? ParentAuditId { get; set; }

    }


    public class AuditGroupResponse
    {

        /// <summary>
        /// Get or set UserId
        /// </summary>
        public string UserName { get; set; }

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
        /// Get or set CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }

    }
}
