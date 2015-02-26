using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Routes {
    class Type {
        [JsonProperty("type")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<Route> Items { get; set; }
    }
}
