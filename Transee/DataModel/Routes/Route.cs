using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Routes {
    class Route {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("routes")]
        public List<LatLon> Routes { get; set; }
    }
}
