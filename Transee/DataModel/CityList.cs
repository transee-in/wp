using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transee.API;
using Windows.Data.Json;

namespace Transee.DataModel {
    public class CityList {
        private static CityList _cityListData = new CityList();

        private List<CityItem> _cities = new List<CityItem>();
        public List<CityItem> Cities {
            get { return this._cities; }
        }

        public static async Task<IEnumerable<CityItem>> GetCitiesAsync() {
            await _cityListData.GetDataAsync();

            return _cityListData.Cities;
        }

        public static async Task<CityItem> GetCityAsync(string Id) {
            await _cityListData.GetDataAsync();
            var matches = _cityListData.Cities.Where((group) => group.Id.Equals(Id));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetDataAsync() {
            if (this._cities.Count != 0) {
                return;
            }

            var request = new Request();
            var jsonData = await request.Get("cities");
            JsonArray jsonArray = JsonArray.Parse(jsonData).GetArray();

            foreach (JsonValue city in jsonArray) {
                CityItem cityElement = new CityItem(city.GetString());
                this.Cities.Add(cityElement);
            }
        }
    }
}
