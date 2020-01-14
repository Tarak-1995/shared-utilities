/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;

namespace PrimeroEdge.SharedUtilities.Api
{
    /// <summary>
    /// Api Response
    /// </summary>
    public class ApiResponse<T>
    {

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Date time
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="success"></param>
        public ApiResponse(T data, string message = null, bool success = true)
        {
            Data = data;
            Message = message;
            Success = success;
        }
    }
}
