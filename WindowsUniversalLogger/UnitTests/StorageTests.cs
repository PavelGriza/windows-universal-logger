using System;
using System.IO;
using System.Linq;
using Windows.Storage;
using WindowsUniversalLogger.Interfaces.Extensions;
using Xunit;

namespace UnitTests
{
    public class StorageTests
    {
        [Theory]
        [InlineData(@"C:\Users\Public\Documents")]
        public async void GetFileSizeTest(string path)
        {
            var rootFolder = await StorageFolder.GetFolderFromPathAsync(path);
            const string testFolderName = "WULTests";
            const string testFileName = "TestFile.dat";

            var testFolder = (await rootFolder.GetFoldersAsync()).FirstOrDefault(f => f.Name == testFolderName);

            if (testFolder == null)
            {
                testFolder = await rootFolder.CreateFolderAsync(testFolderName);
            }

            StorageFile testFile = await testFolder.CreateFileAsync(testFileName, CreationCollisionOption.ReplaceExisting);

            var rnd = new Random((int) DateTime.Now.Ticks);

            // generate random byte array with random size
            var bytes = new byte[rnd.Next(byte.MinValue, byte.MaxValue)];
            rnd.NextBytes(bytes);

            // write byte array to a file
            await FileIO.WriteBytesAsync(testFile, bytes);

            // check GetFreeSpace method
            Assert.True(await testFolder.GetFreeSpace() > 0);

            // get file size in bytes
            ulong fileSize = await testFile.GetFileSize();
            await testFolder.DeleteAsync();

            Assert.Equal((ulong)bytes.Length, fileSize);
        }

        [Theory]
        [InlineData(@"C:\Users\Public\Documents")]
        public async void GetFreeSpaceTest(string path)
        {
            var rootFolder = await StorageFolder.GetFolderFromPathAsync(path);
            var freeSpace = await rootFolder.GetFreeSpace();

            Assert.True(freeSpace > 0);
        }

        [Theory]
        [InlineData(@"C:\Users\Public\Documents")]
        public async void CheckIsFileExistsTest(string path)
        {
            var rootFolder = await StorageFolder.GetFolderFromPathAsync(path);
            const string testFileName = "TestFile.dat";

            StorageFile testFile = await rootFolder.CreateFileAsync(testFileName, CreationCollisionOption.ReplaceExisting);

            Assert.True(await testFile.Exists());

            await testFile.DeleteAsync();

            Assert.False(await testFile.Exists());
        }

        [Theory]
        [InlineData(@"C:\Users\Public\Documents")]
        public async void CheckIsFolderExistsTest(string path)
        {
            var rootFolder = await StorageFolder.GetFolderFromPathAsync(path);
            const string testFolderName = "TestFolder";

            StorageFolder testFolder = await rootFolder.CreateFolderAsync(testFolderName);

            Assert.True(await testFolder.Exists());

            await testFolder.DeleteAsync();

            Assert.False(await testFolder.Exists());
        }

        [Theory]
        [InlineData(@"C:\Users\Public\Documents")]
        public async void GerOrCreateSubfolderTest(string path)
        {
            var rootFolder = await StorageFolder.GetFolderFromPathAsync(path);
            
            var testFolder = await rootFolder.CreateFolderAsync("TestFolder");
            string subfolder1 = "Subfolder1";
            string subfolder2 = "Subfolder2";

            var subfolder = await testFolder.GetOrCreateSubfolderAsync(Path.Combine(subfolder1, subfolder2));

            Assert.True(await subfolder.Exists());

            await testFolder.DeleteAsync();
        }

    }
}
