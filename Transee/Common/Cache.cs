using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Transee.Common {
    class Cache {
        private static Dictionary<string, string> runtimeCache = new Dictionary<string, string>();

        public async void Set(string key, string data) {
            Debug.WriteLine("[Cache] set {0}", key);

            if (runtimeCache.ContainsKey(key)) {
                return;
            }

            try {
                string fileName = CreateKey(key);
                IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                IStorageFile storageFile = await applicationFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                using (Stream stream = await storageFile.OpenStreamForWriteAsync()) {
                    byte[] content = Encoding.UTF8.GetBytes(data);
                    await stream.WriteAsync(content, 0, content.Length);
                    stream.Dispose();
                    runtimeCache.Add(key, data);
                }
            } catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<string> Get(string key) {
            if (runtimeCache.ContainsKey(key)) {
                Debug.WriteLine("[Cache] get from runtime {0}", key);
                return runtimeCache[key];
            }

            try {
                Debug.WriteLine("[Cache] get from filesystem {0}", key);
                string fileName = CreateKey(key);
                IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                IStorageFile storageFile = await applicationFolder.GetFileAsync(fileName);
                IRandomAccessStream accessStream = await storageFile.OpenReadAsync();
                string data = null;
                using (Stream stream = accessStream.AsStreamForRead((int) accessStream.Size)) {
                    byte[] content = new byte[stream.Length];
                    await stream.ReadAsync(content, 0, (int) stream.Length);
                    data = Encoding.UTF8.GetString(content, 0, content.Length);
                    stream.Dispose();
                }
                runtimeCache.Add(key, data);
                return data;
            } catch (Exception e) {
                Debug.WriteLine("[Cache] get exception {0}", e.Message);
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
