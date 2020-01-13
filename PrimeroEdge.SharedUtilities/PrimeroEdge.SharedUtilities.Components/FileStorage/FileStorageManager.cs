/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// IFileStorageManager
    /// </summary>
    public class FileStorageManager:IFileStorageManager
    {

        /// <summary>
        /// fileStorageRepository
        /// </summary>
        private readonly IFileStorageRepository _fileStorageRepository;

        /// <summary>
        /// fileStorageRepository
        /// </summary>
        /// <param name="fileStorageRepository"></param>
        public FileStorageManager(IFileStorageRepository fileStorageRepository)
        {
            if (fileStorageRepository == null)
                throw new ArgumentNullException(nameof(fileStorageRepository));

            _fileStorageRepository = fileStorageRepository;
        }

        /// <summary>
        /// Create file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        ///  <param name="contentType"></param>
        /// <returns></returns>
        public async Task CreateFileAsync(byte[] bytes, string fileName, string contentType)
        {
            await _fileStorageRepository.CreateFileAsync(bytes, fileName, contentType).ConfigureAwait(false);
        }

        /// <summary>
        /// <summary>
        /// Update file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task UpdateFileAsync(byte[] bytes, string fileName, string contentType)
        {
            await _fileStorageRepository.UpdateFileAsync(bytes, fileName, contentType).ConfigureAwait(false);
        }

        /// <summary>
        /// Read file 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<FileStorageData> ReadFileAsync(string fileName)
        {
            return await _fileStorageRepository.ReadFileAsync(fileName).ConfigureAwait(false);
        }


        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteFileAsync(string fileName)
        {
            await _fileStorageRepository.DeleteFileAsync(fileName).ConfigureAwait(false);
        }
    }
}
