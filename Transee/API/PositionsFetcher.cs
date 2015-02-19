using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Transee.DataModel;

namespace Transee.API {
    using PositionListSchema = Dictionary<string, Dictionary<string, List<List<double>>>>;
    using Filter = Dictionary<string, List<string>>;

    class PositionsFetcher {
        public static async Task<List<PositionType>> GetAsync(string city, Filter filter) {
            var request = new Request();
            var jsonData = await request.Post(city, "positions", PrepareFilter(filter));
            var routes = new List<PositionType>();

            PositionListSchema data = JsonConvert.DeserializeObject<PositionListSchema>(jsonData);

            foreach (var position in data) {
                PositionType routeType = new PositionType(position.Key);

                foreach (var number in position.Value) {
                    var latLonsAz = new List<LatLonAz>();
                    var positionItem = new PositionItem();

                    foreach (var rawLatLonAzs in number.Value) {
                        var latLonAz = new LatLonAz(rawLatLonAzs);

                        positionItem.Route.Add(latLonAz);
                    }

                    routeType.Items.Add(number.Key, positionItem);
                }

                routes.Add(routeType);
            }

            return routes;
        }

        private static FormUrlEncodedContent PrepareFilter(Filter filter) {
            var qs = new List<KeyValuePair<string, string>>();

            foreach (var numbers in filter) {
                qs.Add(new KeyValuePair<string, string>("type[]", numbers.Key));

                foreach (var number in numbers.Value) {
                    var key = string.Format("numbers[{0}][]", numbers.Key);
                    qs.Add(new KeyValuePair<string, string>(key, number));
                }
            }

            return new FormUrlEncodedContent(qs);
        }
    }
}
