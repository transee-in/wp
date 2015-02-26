using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel.Cities;

namespace Transee.API {
    class CitiesFetcher {
        public static async Task<Cities> GetAsync() {
            var request = new Request();
            var jsonData = await request.Get("cities");
            var listCities = JsonConvert.DeserializeObject<List<City>>(jsonData);

            return new Cities(listCities);
        }
    }
}
