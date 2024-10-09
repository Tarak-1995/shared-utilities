using System.Collections.Generic;

namespace PrimeroEdge.SharedUtilities.Components
{
    public class MultipleEntitiesAuditGroupResponseContract
    {

        /// <summary>
        /// Get or set UserId
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Get or set EntityTypeId
        /// </summary>
        public string EntityTypeId { get; set; }


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

        /// <summary>
        /// Get or set AuditId.
        /// </summary>
        public Guid AuditId { get; set; }

        /// <summary>
        /// Get or set ParentAuditId.
        /// </summary>
        public Guid? ParentAuditId { get; set; }

    }
}
