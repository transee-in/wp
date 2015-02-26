using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Stations {
    class Item {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("position")]
        private List<double> _position {
            set { this.Position = new LatLon(value); }
        }

        public LatLon Position { get; set; }
    }
}
