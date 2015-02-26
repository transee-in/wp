using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Positions {
    class Item {
        [JsonProperty("gos_id")]
        public string GosID { get; set; }

        [JsonProperty("angle")]
        public int Angle { get; set; }

        [JsonProperty("position")]
        private List<double> _position {
            set { this.Position = new LatLon(value); }
        }

        public LatLon Position { get; set; }
    }
}
