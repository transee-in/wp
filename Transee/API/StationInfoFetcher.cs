using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Transee.DataModel.StationInfo;

namespace Transee.API {
    class StationInfoFetcher {
        public static async Task<Station> GetAsync(string city, string id) {
            var request = new Request();
            var jsonData = await request.Post(city, "positions", PrepareFilter(id));
            var stationInfo = JsonConvert.DeserializeObject<Station>(jsonData);

            return stationInfo;
        }

        private static FormUrlEncodedContent PrepareFilter(string id) {
            var qs = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>(id, id)
            };

            return new FormUrlEncodedContent(qs);
        }
    }
}
