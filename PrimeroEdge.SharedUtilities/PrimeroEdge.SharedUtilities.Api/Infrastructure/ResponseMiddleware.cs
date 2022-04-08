/*
 ***********************************************************************
 * Copyright © 2019 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Cybersoft.Platform.Utilities.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Cybersoft.Platform.Utilities.MiddleWare;

namespace PrimeroEdge.SharedUtilities.Api
{
    /// <summary>
    /// ResponseMiddleware
    /// </summary>
    public class ResponseMiddleware : APIResponseMiddlewareEx
    {
        /// <summary>
        /// ResponseMiddleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="factory"></param>
        /// <param name="logger"></param>
        public ResponseMiddleware(RequestDelegate next, HttpStatusMessageFactory factory, ILogger<APIResponseMiddlewareEx> logger)
            : base(next, factory, logger)
        {

        }
    }
}
