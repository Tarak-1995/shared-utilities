using Cybersoft.Platform.Utilities.MiddleWare;
/*
 ***********************************************************************
 * Copyright © 2019 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Cybersoft.Platform.Contracts;
using Cybersoft.Platform.Data.MongDb;
using Cybersoft.Platform.Utilities.ResponseModels;
using Microsoft.AspNetCore.Http;

namespace PrimeroEdge.SharedUtilities.Api
{
    /// <summary>
    /// ResponseMiddleware
    /// </summary>
    public class ResponseMiddleware : APIResponseMiddleware
    {
        /// <summary>
        /// ResponseMiddleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="mongoDbManager"></param>
        /// <param name="cybersoftLogger"></param>
        public ResponseMiddleware(RequestDelegate next, IMongoDbManager<MessageData> mongoDbManager, ICybersoftLogger cybersoftLogger)
            : base(next, mongoDbManager, cybersoftLogger)
        {

        }
    }
}
