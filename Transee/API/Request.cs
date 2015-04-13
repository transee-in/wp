using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Transee.Common;

namespace Transee.API {
    class Request : IDisposable {
	    private const string Host = "https://transee.in/api/v1/";
	    private readonly HttpClient _client = new HttpClient();
        private readonly Cache _cache = new Cache();

        public async Task<string> GetCity(string city, string method) {
            var url = string.Concat(Host, "cities/", city, "/", method);
            return await Fetch(url);
        }

        public async Task<string> GetCity(string city) {
            var url = string.Concat(Host, "cities/", city);
            return await Fetch(url);
        }

        public async Task<string> Get(string method) {
            var url = string.Concat(Host, method);
            return await Fetch(url);
        }

        public async Task<string> Post(string city, string method, FormUrlEncodedContent body) {
            var url = string.Concat(Host, "cities/", city, "/", method);
            Debug.WriteLine("[Request] post {0}", url);
            var response = await _client.PostAsync(url, body);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        private async Task<string> Fetch(string url) {
            var value = await _cache.Get(url);

            if (value != null) {
                return value;
            } else {
                Debug.WriteLine("[Request] get {0}", url);
                var response = await _client.GetStringAsync(url);
                _cache.Set(url, response);
                return response;
            }
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
