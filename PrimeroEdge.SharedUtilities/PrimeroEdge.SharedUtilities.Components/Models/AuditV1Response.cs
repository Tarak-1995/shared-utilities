using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// AuditResponse
    /// </summary>
    public class AuditV1Response
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

        public List<AuditV1Response> Children { get; set; }

    }


    public class AuditV1GroupResponse
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

        /// <summary>
        /// Get or set AuditId.
        /// </summary>
        public Guid AuditId { get; set; }

        /// <summary>
        /// Get or set ParentAuditId.
        /// </summary>
        public Guid? ParentAuditId { get; set; }

        public List<AuditV1GroupResponse> Children { get; set; }


    }
}
