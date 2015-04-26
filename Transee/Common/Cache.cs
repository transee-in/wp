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
	internal class Cache {
		private static readonly Dictionary<string, string> RuntimeCache = new Dictionary<string, string>();

		public async void Set(string key, string data) {
			Debug.WriteLine("[Cache] set {0}", key);

			if (RuntimeCache.ContainsKey(key)) {
				return;
			}

			var fileName = CreateKey(key);
			IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
			IStorageFile storageFile = await applicationFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
			using (var stream = await storageFile.OpenStreamForWriteAsync()) {
				var content = Encoding.UTF8.GetBytes(data);
				await stream.WriteAsync(content, 0, content.Length);
				stream.Dispose();
				RuntimeCache.Add(key, data);
			}
		}

		public async Task<string> Get(string key) {
			if (RuntimeCache.ContainsKey(key)) {
				Debug.WriteLine("[Cache] get from runtime {0}", key);
				return RuntimeCache[key];
			}

			try {
				Debug.WriteLine("[Cache] get from filesystem {0}", key);
				var fileName = CreateKey(key);
				IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
				IStorageFile storageFile = await applicationFolder.GetFileAsync(fileName);
				IRandomAccessStream accessStream = await storageFile.OpenReadAsync();
				string data;
				using (var stream = accessStream.AsStreamForRead((int) accessStream.Size)) {
					var content = new byte[stream.Length];
					await stream.ReadAsync(content, 0, (int) stream.Length);
					data = Encoding.UTF8.GetString(content, 0, content.Length);
					stream.Dispose();
				}
				RuntimeCache.Add(key, data);
				return data;
			} catch (Exception e) {
				Debug.WriteLine("[Cache] get exception {0}", e.Message);
				return null;
			}
		}

		private static string CreateKey(string str) {
			var hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
			var buffUtf8 = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
			var hashed = hasher.HashData(buffUtf8);
			var hashedString = CryptographicBuffer.EncodeToHexString(hashed);
			return hashedString;
		}
	}
}