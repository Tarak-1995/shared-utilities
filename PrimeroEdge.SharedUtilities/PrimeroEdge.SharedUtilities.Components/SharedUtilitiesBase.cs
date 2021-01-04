using Cybersoft.Platform.Utilities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimeroEdge.SharedUtilities.Components
{
    public abstract class SharedUtilitiesBase<T> where T : SharedUtilitiesBase<T>
    {
        public ResponseEnvelope ResponseEnvelope { get; set; } = new ResponseEnvelope();
        public Pagination PaginationEnvelope { get; set; } 

        public ResponseEnvelope GetResponseEnvelope()
        {
            return ResponseEnvelope;
        }
    }
}
