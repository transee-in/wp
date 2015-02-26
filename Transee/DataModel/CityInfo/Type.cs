using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;

namespace Transee.DataModel.CityInfo {
    class Type {
        private ResourceLoader resourceLoader = new ResourceLoader();

        [JsonProperty("type")]
        public string Id { get; set; }

        [JsonProperty("items")]
        public List<TypeItem> Items { get; set; }

        public string Name {
            get { return resourceLoader.GetString(this.Id); }
        }
    }
}
