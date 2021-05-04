/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */
using Cybersoft.Platform.Data.MongDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// Audit
    /// </summary>
    public class Audit : IMongoEntity
    {
        /// <summary>
        /// Get or set Id
        /// </summary>
        [JsonProperty("_id")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string Id { get; set; }

        /// <summary>
        /// Get or set RegionId
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int RegionId { get; set; }

        /// <summary>
        /// Get or set ModuleId
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string ModuleId { get; set; }

        /// <summary>
        /// Get or set EntityTypeId
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string EntityTypeId { get; set; }

        /// <summary>
        /// Get or set EntityId
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string EntityId { get; set; }

        /// <summary>
        /// Get or set UserId
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Get or set UserId
        /// </summary>
        public string UserName { get; set; }

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
    }


    public class AuditGroup
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
