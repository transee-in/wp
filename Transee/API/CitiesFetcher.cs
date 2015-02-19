using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel;

namespace Transee.API {
    using CityListSchema = List<string>;

    class CitiesFetcher {
        public static async Task<List<CityItem>> GetAsync() {
            var request = new Request();
            var jsonData = await request.Get("cities");
            var cities = new List<CityItem>();

            CityListSchema data = JsonConvert.DeserializeObject<CityListSchema>(jsonData);

            foreach (var id in data) {
                var item = new CityItem(id);
                cities.Add(item);
            }

            return cities;
        }
    }
}
