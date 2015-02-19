using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel;

namespace Transee.API {
    using RouteListSchema = Dictionary<string, Dictionary<string, List<List<double>>>>;

    class RoutesFetcher {
        public static async Task<List<RouteType>> GetAsync(string city) {
            var request = new Request();
            var jsonData = await request.Get(city, "routes");
            var routes = new List<RouteType>();
            
            RouteListSchema data = JsonConvert.DeserializeObject<RouteListSchema>(jsonData);

            foreach (var route in data) {
                RouteType routeType = new RouteType(route.Key);

                foreach (var number in route.Value) {
                    var latLons = new List<LatLon>();
                    var routeItem = new RouteItem();

                    foreach (var rawLatLons in number.Value) {
                        var latLon = new LatLon(rawLatLons);

                        routeItem.Route.Add(latLon);
                    }

                    routeType.Items.Add(number.Key, routeItem);
                }

                routes.Add(routeType);
            }

            return routes;
        }
    }
}
