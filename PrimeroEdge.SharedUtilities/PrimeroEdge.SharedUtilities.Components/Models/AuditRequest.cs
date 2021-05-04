/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System.Text.Json.Serialization;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// AuditRequest
    /// </summary>
    public class AuditRequest 
    {
        /// <summary>
        /// Get or set ModuleId
        /// </summary>
       [JsonIgnore]
        public string ModuleId { get; set; }

        /// <summary>
        /// Get or set EntityTypeId
        /// </summary>
        [JsonIgnore]
        public string EntityTypeId { get; set; }

        /// <summary>
        /// EntityId
        /// </summary>
        [JsonIgnore]
        public string EntityId { get; set; }

        /// <summary>
        /// Field
        /// </summary>
        public string Field { get; set; }


        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// PageNumber
        /// </summary>

        public int PageNumber { get; set; }
    }
}
