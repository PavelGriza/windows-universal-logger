using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace WindowsUniversalLogger.Interfaces.Extensions
{
    public static class StorageExtensions
    {
        private const string FreeSpace = "System.FreeSpace";

        public static async Task<ulong> GetFreeSpace(this IStorageItem storageItem)
        {
            var properties = await storageItem.GetBasicPropertiesAsync();
            var filteredProperties = await properties.RetrievePropertiesAsync(new[] {FreeSpace});
            var freeSpace = filteredProperties[FreeSpace];

            return (UInt64) freeSpace;
        }

        public static async Task<ulong> GetFileSize(this IStorageItem storageItem)
        {
            if (storageItem == null)
                throw new ArgumentException("Parameter cannot be null.", "storageItem");

            var fileProperties = await storageItem.GetBasicPropertiesAsync();
            var size = fileProperties.Size;
            return size;
        }

        /// <summary>
        /// Parses subfolderPath and gets or creates new sub folders with predetermined depth relation 
        /// </summary>
        /// <param name="rootFolder">The root folder</param>
        /// <param name="subfolderPath">Sub folder path where each folder is separated by "\\" symbol</param>
        /// <returns>Sub folder</returns>
        public static async Task<IStorageFolder> GetOrCreateSubfolderAsync(this IStorageFolder rootFolder, string subfolderPath)
        {
            var subFolders = subfolderPath.Split('\\').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            IStorageFolder resultFolder = rootFolder;

            foreach (var folder in subFolders)
            {
                resultFolder = await resultFolder.GetOrCreateFolderAsync(folder) as StorageFolder;
            }

            return resultFolder;
        }

        /// <summary>
        /// Gets existing or cteates new folder with specific name as sub folder to the rootFolder
        /// </summary>
        /// <param name="rootFolder">Root folder</param>
        /// <param name="folderName">Name of sub folder</param>
        /// <returns>Sub folder</returns>
        public static async Task<IStorageFolder> GetOrCreateFolderAsync(this IStorageFolder rootFolder,
            string folderName)
        {
            var subFolder = await rootFolder.TryGetItemAsync(folderName) as IStorageFolder ??
                            await rootFolder.CreateFolderAsync(folderName);

            return subFolder;
        }

        public static async Task<IStorageItem> TryGetItemAsync(this IStorageFolder folder, string name)
        {
            return (await folder.GetItemsAsync()).FirstOrDefault(item => item.Name == name);
        }

        /// <summary>
        /// Not so effective but the simpliest way to check is the file exists
        /// </summary>
        /// <param name="file">File for check</param>
        /// <returns></returns>
        public static async Task<bool> Exists(this IStorageFile file)
        {
            if (file == null)
            {
                return false;
            }

            StorageFile current;

            try
            {
                current = await StorageFile.GetFileFromPathAsync(file.Path);
            }
            catch (FileNotFoundException e)
            {
                return false;
            }

            return current != null;
        }

        public static async Task<bool> Exists(this IStorageFolder folder)
        {
            if (folder == null)
            {
                return false;
            }

            StorageFolder current;

            try
            {
                current = await StorageFolder.GetFolderFromPathAsync(folder.Path);
            }
            catch (FileNotFoundException e)
            {
                return false;
            }

            return current != null;
        }
    }
}