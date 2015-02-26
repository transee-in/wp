using Newtonsoft.Json;

namespace Transee.DataModel.CityInfo {
    class TypeItem {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public TypeItem(string id, string name) {
            this.Id = id;
            this.Name = name;
        }
    }
}
