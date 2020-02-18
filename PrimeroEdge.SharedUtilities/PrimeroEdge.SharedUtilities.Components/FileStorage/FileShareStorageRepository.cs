/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using MimeTypes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// File share storage repository
    /// </summary>
    public class FileShareStorageRepository : IFileStorageRepository
    {

        private readonly Lazy<Task<FileStorageSettings>> _fileStorageSettings;

        /// <summary>
        /// File share storage repository
        /// </summary>
        /// <param name="fileStorageSettings"></param>
        public FileShareStorageRepository(Lazy<Task<FileStorageSettings>> fileStorageSettings)
        {
            _fileStorageSettings = fileStorageSettings;
        }

        /// <summary>
        /// Create file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task CreateFileAsync(byte[] bytes, string fileName, string contentType)
        {
           var fileSharePath = (await _fileStorageSettings.Value.ConfigureAwait(false)).FileSharePath;
            var path = Path.Combine(fileSharePath, fileName);
            await File.WriteAllBytesAsync(path, bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteFileAsync(string fileName)
        {
            var fileSharePath = (await _fileStorageSettings.Value.ConfigureAwait(false)).FileSharePath;
            var path = Path.Combine(fileSharePath, fileName);

            if (File.Exists(path))
                File.Delete(path);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Read file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<FileStorageData> ReadFileAsync(string fileName)
        {
            var fileSharePath = (await _fileStorageSettings.Value.ConfigureAwait(false)).FileSharePath;
            var path = Path.Combine(fileSharePath, fileName);
            
            if (File.Exists(path))
                return new FileStorageData();

            var data = await File.ReadAllBytesAsync(path).ConfigureAwait(false);
            var contentType = MimeTypeMap.GetMimeType(Path.GetExtension(path));
            return new FileStorageData() { Content = data, ContentType = contentType };
        }

        /// <summary>
        /// Update file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task UpdateFileAsync(byte[] bytes, string fileName, string contentType)
        {
            var fileSharePath = (await _fileStorageSettings.Value.ConfigureAwait(false)).FileSharePath;
            var path = Path.Combine(fileSharePath, fileName);
            await File.WriteAllBytesAsync(path, bytes).ConfigureAwait(false);
        }
    }
}
