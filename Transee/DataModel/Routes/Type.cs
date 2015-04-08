using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Routes {
    class Type {
        [JsonProperty("type")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<Route> Items { get; set; }

        public Route GetRouteById(string id) {
            Route foundedItem = null;

            foreach (var item in Items) {
                if (item.Id == id) {
                    foundedItem = item;
                    break;
                }
            }

            return foundedItem;
        }
    }
}
