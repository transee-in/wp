using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Transee.Common;

namespace Transee.API {
	internal class Request : IDisposable {
		private const string Host = "https://transee.in/api/v1/";
		private readonly HttpClient _client = new HttpClient();
		private readonly Cache _cache = new Cache();

		public async Task<string> GetCity(string city, string method) {
			var url = $"{Host}cities/{city}/{method}";
			return await Fetch(url);
		}

		public async Task<string> GetCity(string city) {
			var url = $"{Host}cities/{city}";
			return await Fetch(url);
		}

		public async Task<string> Get(string method) {
			var url = $"{Host}{method}";
			return await Fetch(url);
		}

	    public async Task<string> Post(string method) {
            var url = $"{Host}{method}";
            var body = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
            var response = await _client.PostAsync(url, body);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

		public async Task<string> Post(string city, string method, FormUrlEncodedContent body) {
			var url = $"{Host}cities/{city}/{method}";
			var response = await _client.PostAsync(url, body);
			var responseString = await response.Content.ReadAsStringAsync();
			return responseString;
		}

		private async Task<string> Fetch(string url) {
			var fromCache = await _cache.Get(url);
			if (fromCache != null) {
				return fromCache;
			}

			var response = await _client.GetStringAsync(url);
			_cache.Set(url, response);
			return response;
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				_client.Dispose();
			}
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}