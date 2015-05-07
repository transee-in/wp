using System.Collections.Generic;
using Newtonsoft.Json;

namespace Transee.DataModel.StationInfo {
    internal class Station {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("transports")]
        public List<Transport> Transports { get; set; }

        [JsonProperty("forecasts")]
        public List<Forecast> Forecasts { get; set; }
    }
}
