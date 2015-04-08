using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace Transee.DataModel.Routes {
    class Route {
        [JsonProperty("id")]
        public string Id { get; set; }

        public List<LatLon> Routes { get; set; }

        [JsonProperty("route")]
        private List<List<double>> _route {
            set {
                this.Routes = new List<LatLon>();

                foreach (var latLon in value) {
                    this.Routes.Add(new LatLon(latLon));
                }
            }
        }

        public MapPolyline CreateMapPolyline() {
            var line = new MapPolyline();
            var coordinates = new List<BasicGeoposition>();

            foreach (var route in Routes) {
                coordinates.Add(new BasicGeoposition() {
                    Latitude = route.Lat, Longitude = route.Lon
                });
            }

            line.Path = new Geopath(coordinates);

            return line;
        }
    }
}
