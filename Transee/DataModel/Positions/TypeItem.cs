using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Positions {
    class TypeItem {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
}
