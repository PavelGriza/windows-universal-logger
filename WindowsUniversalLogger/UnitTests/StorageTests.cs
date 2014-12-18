using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public async void CteateSubfolderIfNotExistsTest()
        {
            StorageFolder rootFolder = await StorageFolder.GetFolderFromPathAsync(@"C:\Users\Public\Documents");
            string localFolderPath = Path.Combine("Log", "FileLog");
        }

        private Task<IStorageFolder> GetFolder(IStorageFolder rootFolder, string subfolderPath)
        {
            string[] arr = subfolderPath.Split('\\');
            return null;
        }
    }
}
