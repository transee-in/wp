using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel.Routes;

namespace Transee.API {
    class RoutesFetcher {
        public static async Task<Types> GetAsync(string city) {
            var request = new Request();
            var jsonData = await request.GetCity(city, "routes");
            var listRoutes = JsonConvert.DeserializeObject<List<DataModel.Routes.Type>>(jsonData);

            return new Types(listRoutes);
        }
    }
}
