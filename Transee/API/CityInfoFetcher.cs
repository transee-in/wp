using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel.CityInfo;

namespace Transee.API {
    class CityInfoFetcher {
        public static async Task<Types> GetAsync(string city) {
            var request = new Request();
            var jsonData = await request.GetCity(city);
            var listTransports = JsonConvert.DeserializeObject<List<DataModel.CityInfo.Type>>(jsonData);

            return new Types(listTransports);
        }
    }
}
