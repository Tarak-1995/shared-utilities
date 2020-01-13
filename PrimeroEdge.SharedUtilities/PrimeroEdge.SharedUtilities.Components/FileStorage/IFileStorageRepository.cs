/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// IFileStorageRepository
    /// </summary>
    public interface IFileStorageRepository
    {
        /// <summary>
        /// Create file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        ///  <param name="contentType"></param>
        /// <returns></returns>
        Task CreateFileAsync(byte[] bytes, string fileName, string contentType);

        /// <summary>
        /// <summary>
        /// Update file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Task UpdateFileAsync(byte[] bytes, string fileName, string contentType);

        /// <summary>
        /// Read file 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<FileStorageData> ReadFileAsync(string fileName);

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task DeleteFileAsync(string fileName);
    }
}
