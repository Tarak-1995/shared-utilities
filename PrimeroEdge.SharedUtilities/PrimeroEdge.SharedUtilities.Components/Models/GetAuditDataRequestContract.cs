using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components.Models
{
    public class GetAuditDataRequestContract
    {
        public string ModuleId { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public List<EntityTypes> Entities { get; set; }
    }

    public class EntityTypes
    {
        public string EntityTypeId { get; set; }

        public string EntityId { get; set; }
    }
}
