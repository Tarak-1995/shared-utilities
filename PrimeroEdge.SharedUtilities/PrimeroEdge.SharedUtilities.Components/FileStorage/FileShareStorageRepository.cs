/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using MimeTypes;
using System.IO;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// File share storage repository
    /// </summary>
    public class FileShareStorageRepository : IFileStorageRepository
    {
        /// <summary>
        /// File share path
        /// </summary>
        private readonly string _fileSharePath;

        /// <summary>
        /// File share storage repository
        /// </summary>
        /// <param name="fileStorageSettings"></param>
        public FileShareStorageRepository(FileStorageSettings fileStorageSettings)
        {
            _fileSharePath = fileStorageSettings.FileSharePath;
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
            var path = Path.Combine(_fileSharePath, fileName);
            await File.WriteAllBytesAsync(path, bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteFileAsync(string fileName)
        {
            var path = Path.Combine(_fileSharePath, fileName);

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
            var path = Path.Combine(_fileSharePath, fileName);
            
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
            var path = Path.Combine(_fileSharePath, fileName);
            await File.WriteAllBytesAsync(path, bytes).ConfigureAwait(false);
        }
    }
}
