using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Transee.Common;

namespace Transee.DataModel.Routes {
    class Route {
        [JsonProperty("id")]
        public string Id { get; set; }

        public List<LatLon> Routes { get; set; }

        [JsonProperty("route")]
        internal List<List<double>> _route {
            set {
                Routes = new List<LatLon>();

                foreach (var latLon in value) {
                    Routes.Add(new LatLon(latLon));
                }
            }
        }

        public MapPolyline CreateMapPolyline(string typeName) {
            var line = new MapPolyline();
            var coordinates = Routes.Select(route => new BasicGeoposition {
	            Latitude = route.Lat, Longitude = route.Lon
            }).ToList();

	        line.Path = new Geopath(coordinates);
            line.StrokeColor = new ColorGenerator(typeName + Id).Generate();
            line.StrokeThickness = 5;

            return line;
        }
    }
}
