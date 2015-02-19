using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel;

namespace Transee.API {
    using TransportListSchema = Dictionary<string, Dictionary<string, string>>;

    class TransportsFetcher {
        public static async Task<List<TransportType>> GetAsync(string city) {
            var request = new Request();
            var jsonData = await request.Get(city);
            var transports = new List<TransportType>();

            TransportListSchema data = JsonConvert.DeserializeObject<TransportListSchema>(jsonData);

            foreach (var type in data) {
                var transportType = new TransportType(type.Key);

                foreach (var number in type.Value) {
                    var item = new TransportItem(number.Key, number.Value);
                    transportType.Elements.Add(item);
                }

                transports.Add(transportType);
            }

            return transports;
        }
    }
}
