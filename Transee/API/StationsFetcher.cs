using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel;

namespace Transee.API {
    using StationListSchema = Dictionary<string, List<List<double>>>;

    class StationsFetcher {
        public static async Task<List<StationType>> GetAsync(string city) {
            var request = new Request();
            var jsonData = await request.Get(city, "stations");
            var stations = new List<StationType>();

            StationListSchema data = JsonConvert.DeserializeObject<StationListSchema>(jsonData);

            foreach (var station in data) {
                StationType stationType = new StationType(station.Key);

                foreach (var rawLatLons in station.Value) {
                    var latLon = new LatLon(rawLatLons);
                    stationType.Items.Add(latLon);
                }

                stations.Add(stationType);
            }

            return stations;
        }
    }
}
