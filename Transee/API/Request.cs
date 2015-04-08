using System;
using System.Net.Http;
using System.Threading.Tasks;
using Transee.Common;

namespace Transee.API {
    class Request : IDisposable {
        private string host = "https://transee.in/api/v1/";
        private HttpClient client = new HttpClient();
        private Cache cache = new Cache();

        public async Task<string> GetCity(string city, string method) {
            var url = string.Concat(host, "cities/", city, "/", method);
            return await this.Fetch(url);
        }

        public async Task<string> GetCity(string city) {
            var url = string.Concat(host, "cities/", city);
            return await this.Fetch(url);
        }

        public async Task<string> Get(string method) {
            var url = string.Concat(host, method);
            return await this.Fetch(url);
        }

        public async Task<string> Post(string city, string method, FormUrlEncodedContent body) {
            var url = string.Concat(host, "cities/", city, "/", method);
            var response = await client.PostAsync(url, body);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        private async Task<string> Fetch(string url) {
            var value = await cache.Get(url);

            if (value != null) {
                return value;
            } else {
                var response = await client.GetStringAsync(url);
                var responseString = response.ToString();
                cache.Set(url, responseString);
                return responseString;
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                client.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
