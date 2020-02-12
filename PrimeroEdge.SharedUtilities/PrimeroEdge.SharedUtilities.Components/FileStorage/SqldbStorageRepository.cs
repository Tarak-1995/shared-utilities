/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */
using Cybersoft.Platform.Data.Sql;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.Components
{
    public class SqldbStorageRepository: IFileStorageRepository
    {
        /// <summary>
        /// sqlDbManager
        /// </summary>
        private readonly Lazy<Task<ISqlDbManager>> _sqlDbManager;

        private const string ReadFileSql = "[dbo].[NF_File_Read]";
        private const string WriteFileSql = "[dbo].[NF_File_Create]";
        private const string UpdateFileSql = "[dbo].[NF_File_Update]";
        private const string DeleteFileSql = "[dbo].[NF_File_Delete]";


        /// <summary>
        /// ModuleRepository
        /// </summary>
        /// <param name="sqlDbManager"></param>
        public SqldbStorageRepository(Lazy<Task<ISqlDbManager>> sqlDbManager)
        {
            if (sqlDbManager == null)
                throw new ArgumentNullException(nameof(sqlDbManager));

            _sqlDbManager = sqlDbManager;
        }

        /// <summary>
        /// Read file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<FileStorageData> ReadFileAsync(string fileName)
        {
            var paramValues = new SqlParameter[] { new SqlParameter("@FileName", fileName) };
            var sqlDbManager = await _sqlDbManager.Value.ConfigureAwait(false);
            var data = await sqlDbManager.GetDataAsync<FileStorageData>(ReadFileSql, paramValues).ConfigureAwait(false);
            return data.FirstOrDefault();
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
            var paramValues = new SqlParameter[] {
                new SqlParameter("@Content ", bytes),
                new SqlParameter("@FileName", fileName),
                new SqlParameter("@ContentType", contentType)
            };
            var sqlDbManager = await _sqlDbManager.Value.ConfigureAwait(false);
            await sqlDbManager.CreateAsync(WriteFileSql, paramValues).ConfigureAwait(false);
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
            var paramValues = new SqlParameter[]{
                new SqlParameter("@Content ", bytes),
                new SqlParameter("@FileName", fileName),
                new SqlParameter("@ContentType", contentType)
            };
            var sqlDbManager = await _sqlDbManager.Value.ConfigureAwait(false);
            await sqlDbManager.UpdateAsync(UpdateFileSql, paramValues).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteFileAsync(string fileName)
        {
            var paramValues = new SqlParameter[] { new SqlParameter("@FileName", fileName) };
            var sqlDbManager = await _sqlDbManager.Value.ConfigureAwait(false);
            await sqlDbManager.UpdateAsync(DeleteFileSql, paramValues).ConfigureAwait(false);
        }
    }
}
