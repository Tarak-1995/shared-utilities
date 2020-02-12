/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeroEdge.SharedUtilities.Components;

namespace PrimeroEdge.SharedUtilities.Api.Controllers
{
    /// <summary>
    /// FileStorageController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        /// <summary>
        /// ModuleManager
        /// </summary>
        private readonly Lazy<Task<IFileStorageManager>> _fileStorageManager;

        /// <summary>
        ///  FileStorage controller constructor
        /// </summary>
        /// <param name="fileStorageManager"></param>
        public FileStorageController(Lazy<Task<IFileStorageManager>> fileStorageManager)
        {
            if (fileStorageManager == null)
                throw new ArgumentNullException(nameof(fileStorageManager));

            _fileStorageManager = fileStorageManager;
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("{fileName}/download")]
        public async Task<ActionResult> DownloadFile(string fileName)
        {

            var fileStorageManager = await _fileStorageManager.Value;
            var data = await fileStorageManager.ReadFileAsync(fileName);

            if(data.Content == null || string.IsNullOrWhiteSpace(data.ContentType))
             return  NotFound();

            return new FileContentResult(data.Content, data.ContentType)
            {
                FileDownloadName = fileName
            };
        }

        /// <summary>
        /// View image
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("{fileName}/image")]
        public async Task<ActionResult> ViewImage(string fileName)
        {
            var fileStorageManager = await _fileStorageManager.Value;
            var data = await fileStorageManager.ReadFileAsync(fileName);

            if (data.Content == null || string.IsNullOrWhiteSpace(data.ContentType))
                return NotFound();

            return File(data.Content, data.ContentType);
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse<string>> Post(IFormFile file)
        {
            var data = GetFileData(file);
            var fileName = $"{Guid.NewGuid()}_{file.FileName}"; //unique file name to avoid overwrite

            var fileStorageManager = await _fileStorageManager.Value;
            await fileStorageManager.CreateFileAsync(data, fileName, file.ContentType);
            return new ApiResponse<string>(fileName);
        }

        /// <summary>
        /// Update a file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPut("{fileName}")]
        public async Task<ApiResponse<string>> Put(IFormFile file, string fileName)
        {
            var data = GetFileData(file);
            var fileStorageManager = await _fileStorageManager.Value;
            await fileStorageManager.UpdateFileAsync(data, fileName, file.ContentType);
            return new ApiResponse<string>(fileName);
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpDelete("{fileName}")]
        public async Task Delete(string fileName)
        {
            var fileStorageManager = await _fileStorageManager.Value;
            await fileStorageManager.DeleteFileAsync(fileName);
        }


        /// <summary>
        /// Get file data
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private byte[] GetFileData(IFormFile file)
        {
            byte[] bytes = null;
            using (var stream = file.OpenReadStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    bytes = reader.ReadBytes((int)stream.Length);
                }
            }
            return bytes;
        }
    }
}