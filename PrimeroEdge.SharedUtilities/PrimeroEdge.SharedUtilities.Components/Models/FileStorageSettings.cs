/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

namespace PrimeroEdge.SharedUtilities.Components
{
    /// <summary>
    /// FileStorageSettings
    /// </summary>
    public class FileStorageSettings
    {
        /// <summary>
        /// SectionName
        /// </summary>
        public static string SectionName { get; set; } = nameof(FileStorageSettings);

        /// <summary>
        /// StorageType
        /// </summary>
        public FileStorageType StorageType { get; set; }

        /// <summary>
        /// SqlConnStringName
        /// </summary>
        public string SqlConnStringName { get; set; }

        /// <summary>
        /// BlobConnString
        /// </summary>
        public string BlobConnString { get; set; }


        /// <summary>
        /// BlobContainer
        /// </summary>
        public string BlobContainer { get; set; }

        /// <summary>
        /// FileSharePath
        /// </summary>
        public string FileSharePath { get; set; }
    }

    /// <summary>
    /// File storage data
    /// </summary>
    public class FileStorageData
    {
        /// <summary>
        /// Content
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// ContentType
        /// </summary>
        public string ContentType { get; set; }
    }

    /// <summary>
    /// File storage type
    /// </summary>
    public enum FileStorageType
    {
        None,
        BlobStorage,
        FlieShare,
        SqlDataBase
    }
}
