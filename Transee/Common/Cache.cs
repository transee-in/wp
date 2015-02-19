using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Transee.Common {
    class Cache {
        public async void Set(string key, string data) {
            try {
                string fileName = CreateKey(key);
                IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                IStorageFile storageFile = await applicationFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                using (Stream stream = await storageFile.OpenStreamForWriteAsync()) {
                    byte[] content = Encoding.UTF8.GetBytes(data);
                    await stream.WriteAsync(content, 0, content.Length);
                }
            } catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<String> Get(string key) {
            try {
                string fileName = CreateKey(key);
                IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                IStorageFile storageFile = await applicationFolder.GetFileAsync(fileName);
                IRandomAccessStream accessStream = await storageFile.OpenReadAsync();
                string text = null;
                using (Stream stream = accessStream.AsStreamForRead((int) accessStream.Size)) {
                    byte[] content = new byte[stream.Length];
                    await stream.ReadAsync(content, 0, (int) stream.Length);
                    text = Encoding.UTF8.GetString(content, 0, content.Length);
                }
                return text;
            } catch (Exception) {
                return null;
            }
        }

        private string CreateKey(string str) {
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buffUTF8 = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            IBuffer hashed = hasher.HashData(buffUTF8);
            string hashedString = CryptographicBuffer.EncodeToHexString(hashed);
            return hashedString;
        }
    }
}
