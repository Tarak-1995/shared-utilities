/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// Blob storage service
    /// </summary>
    public class BlobStorageRepository : IFileStorageRepository
    {
        /// <summary>
        /// Cloud blob container
        /// </summary>
        private CloudBlobContainer _cloudBlobContainer;

        private readonly Lazy<Task<FileStorageSettings>> _fileStorageSettings;

        /// <summary>
        /// Blob storage repository
        /// </summary>
        /// <param name="fileStorageSettings"></param>
        public BlobStorageRepository(Lazy<Task<FileStorageSettings>> fileStorageSettings)
        {
            _fileStorageSettings = fileStorageSettings;
            
        }

        /// <summary>
        /// Read file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<FileStorageData> ReadFileAsync(string fileName)
        {
            byte[] fileData = null;
            string contentType = null;

            var cloudBlobContainer = await GetCloudBlobContainer().ConfigureAwait(false);
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            var fileExits = await blockBlob.ExistsAsync().ConfigureAwait(false);

            if (fileExits)
            {
                contentType = blockBlob.Properties.ContentType;
                using (var memoryStream = new MemoryStream())
                {
                    await blockBlob.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);
                    fileData = memoryStream.ToArray();
                }
            }

            return new FileStorageData() { Content = fileData, ContentType = contentType };
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
            var cloudBlobContainer = await GetCloudBlobContainer().ConfigureAwait(false);
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = contentType;
            await blockBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
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
            var cloudBlobContainer = await GetCloudBlobContainer().ConfigureAwait(false);
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = contentType;
            await blockBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteFileAsync(string fileName)
        {
            var cloudBlobContainer = await GetCloudBlobContainer().ConfigureAwait(false);
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            await blockBlob.DeleteIfExistsAsync().ConfigureAwait(false);
        }


        private async Task<CloudBlobContainer> GetCloudBlobContainer()
        {
            if (_cloudBlobContainer == null)
            {
                var fileStorageSettings = await _fileStorageSettings.Value;
                var cloudStorageAccount = CloudStorageAccount.Parse(fileStorageSettings.BlobConnString);
                var blobClient = cloudStorageAccount.CreateCloudBlobClient();
                _cloudBlobContainer = blobClient.GetContainerReference(fileStorageSettings.BlobContainer);
            }

            return _cloudBlobContainer;
        }
    }
}
