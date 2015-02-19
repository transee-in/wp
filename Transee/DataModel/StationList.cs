using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.API;
using Windows.Data.Json;

namespace Transee.DataModel {
    class StationList {
        private static StationList _stationListData = new StationList();

        private List<StationType> _stations = new List<StationType>();
        public List<StationType> Stations {
            get { return this._stations; }
        }

        public static async Task<IEnumerable<StationType>> GetStationsAsync(string city) {
            await _stationListData.GetDataAsync(city);

            return _stationListData.Stations;
        }

        private async Task GetDataAsync(string city) {
            if (this._stations.Count != 0) {
                return;
            }

            var request = new Request();
            var jsonData = await request.Get(city, "stations");
            JsonObject jsonObject = JsonObject.Parse(jsonData).GetObject();

            foreach (KeyValuePair<string, IJsonValue> obj in jsonObject) {
                var type = obj.Key;
                var stations = obj.Value.GetArray();
                var stationTypeList = new StationType(type);

                foreach (JsonArray latLon in stations) {
                    var stationItem = new LatLon(latLon[0].GetNumber(), latLon[1].GetNumber());

                    stationTypeList.Items.Add(stationItem);
                }

                this._stations.Add(stationTypeList);
            }
        }
    }
}
