/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cybersoft.Platform.DocumentStorage;
using Cybersoft.Platform.Utilities.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// IDocumentStore
        /// </summary>
        private readonly IDocumentStore _documentStore;

        /// <summary>
        ///  FileStorage controller constructor
        /// </summary>
        /// <param name="documentStore"></param>
        public FileStorageController(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="configReferenceId"></param>
        /// <param name="fileReferenceId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("{moduleId}/{configReferenceId}/{fileReferenceId}/download")]
        public async Task<ActionResult> DownloadFileAsync(int moduleId, string configReferenceId, string fileReferenceId, string fileName)
        {
            var file = await _documentStore.GetDocumentAsync(fileReferenceId, configReferenceId, fileName.Trim(), moduleId);

            if (file?.Data == null || string.IsNullOrWhiteSpace(file.ContentType))
                return NotFound();

            return new FileContentResult(file.Data, file.ContentType)
            {
                FileDownloadName = fileName
            };
        }

        /// <summary>
        /// View File
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="configReferenceId"></param>
        /// <param name="fileReferenceId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("{moduleId}/{configReferenceId}/{fileReferenceId}/view")]
        public async Task<ActionResult> ViewFileAsync(int moduleId, string configReferenceId, string fileReferenceId, string fileName)
        {
            var file = await _documentStore.GetDocumentAsync(fileReferenceId, configReferenceId, fileName.Trim(),
                moduleId);

            if (file?.Data == null || string.IsNullOrWhiteSpace(file.ContentType))
                return NotFound();

            if (file.ContentType.ToLower().Contains("video"))
                return new FileStreamResult(new MemoryStream(file.Data), file.ContentType);

            return File(file.Data, file.ContentType);
        }
        
        /// <summary>
        /// Delete Files
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="configReferenceId"></param>
        /// <param name="fileReferenceId"></param>
        /// <returns></returns>
        [HttpDelete("{moduleId}/{configReferenceId}/{fileReferenceId}/delete")]
        public async Task ViewFileAsync(int moduleId, string configReferenceId, string fileReferenceId)
        {
            await _documentStore.DeleteDocumentsAsync(fileReferenceId, configReferenceId, moduleId);
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="moduleId"></param>
        /// <param name="configReferenceId"></param>
        /// <param name="fileReferenceId"></param>
        /// <returns></returns>
        [HttpPost("{moduleId}/{configReferenceId}/{fileReferenceId}/Upload")]
        public async Task<List<ValidationInfo>> PostAsync(IFormFile file, int moduleId, string configReferenceId, string fileReferenceId)
        {
            return await _documentStore.SaveDocumentAsync(new Document(fileReferenceId, file), "326", configReferenceId, moduleId);
        }

    }
}