using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Transee.DataModel.Routes {
    class Type {
        [JsonProperty("type")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<Route> Items { get; set; }

        public Route GetRouteById(string id) => Items.FirstOrDefault(item => item.Id == id);
    }
}
