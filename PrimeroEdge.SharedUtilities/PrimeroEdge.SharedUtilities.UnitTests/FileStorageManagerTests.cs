/*
 ***********************************************************************
 * Copyright © 2020 Cybersoft Technologies, Inc. All rights reserved.
 * Unauthorized copying of this file is strictly prohibited.
 ***********************************************************************
 */

using NSubstitute;
using NUnit.Framework;
using PrimeroEdge.SharedUtilities.Components;
using System;
using System.Threading.Tasks;

namespace PrimeroEdge.SharedUtilities.UnitTests
{
    public class FileStorageManagerTests
    {
        private IFileStorageRepository _fileStorageRepository;
        private IFileStorageManager _fileStorageManager;

        private readonly FileStorageData _fileStorageData = new FileStorageData
        {
            Content =new byte[] {0,1,0},
            ContentType = "test"
        };

        [SetUp]
        public void SetUp()
        {
            _fileStorageRepository = Substitute.For<IFileStorageRepository>();
            _fileStorageManager = new FileStorageManager(_fileStorageRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _fileStorageRepository = null;
            _fileStorageManager = null;
        }


        [Test]
        public void FileStorageManagerConstructor_WhenMissingFileStorageRepository_ShouldThrowError()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new FileStorageManager(null));
            Assert.That(exception.ParamName, Is.EqualTo("fileStorageRepository"));
        }

        [Test]
        public void FileStorageManagerConstructor_WhenAllValidArguments_ShouldReturnInstance()
        {
            Assert.DoesNotThrow(() => new FileStorageManager(_fileStorageRepository));
        }

        [Test]
        public async Task CreateFileAsyncTest()
        {
            _fileStorageRepository.CreateFileAsync(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);
            await _fileStorageManager.CreateFileAsync(_fileStorageData.Content, "", _fileStorageData.ContentType).ConfigureAwait(false);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task UpdateFileAsyncTest()
        {
            _fileStorageRepository.UpdateFileAsync(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);
            await _fileStorageManager.UpdateFileAsync(_fileStorageData.Content, "", _fileStorageData.ContentType).ConfigureAwait(false);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task ReadFileAsyncTest()
        {
            _fileStorageRepository.ReadFileAsync(Arg.Any<string>()).Returns(Task.FromResult(_fileStorageData));
            var data = await _fileStorageManager.ReadFileAsync("123").ConfigureAwait(false);
            Assert.IsTrue(data.ContentType.Equals(_fileStorageData.ContentType));
        }

        [Test]
        public async Task DeleteFileAsyncTest()
        {
            _fileStorageRepository.DeleteFileAsync(Arg.Any<string>()).Returns(Task.CompletedTask);
            var data = await _fileStorageManager.ReadFileAsync("123").ConfigureAwait(false);
            Assert.IsTrue(true);
        }
    }
}
