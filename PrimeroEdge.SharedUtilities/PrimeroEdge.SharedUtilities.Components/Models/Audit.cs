/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */
using Cybersoft.Platform.Data.MongDb;
using System;
using System.ComponentModel.DataAnnotations;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// Audit
    /// </summary>
    public class Audit : MongoBaseDocument
    {

        /// <summary>
        /// Get or set RegionId
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Get or set EntityTypeId
        /// </summary>
        public EntityType EntityTypeId { get; set; }

        /// <summary>
        /// Get or set EntityId
        /// </summary>
        [Required]
        public int EntityId { get; set; }

        /// <summary>
        /// Get or set DataSourceId
        /// </summary>
        public int? DataSourceId { get; set; }

        /// <summary>
        /// Get or set UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Get or set CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Get or set ChangeFromLocation
        /// </summary>
        public string ChangeFromLocation { get; set; }


        /// <summary>
        /// Get or set Field
        /// </summary>
        [Required]
        public string Field { get; set; }


        /// <summary>
        /// Get or set OldValue
        /// </summary>
        public string OldValue { get; set; }


        /// <summary>
        /// Get or set NewValue
        /// Get or set NewValue
        /// </summary>
        public string NewValue { get; set; }

    }
}
