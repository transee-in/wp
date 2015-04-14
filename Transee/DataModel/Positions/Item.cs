﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Positions {
    class Item {
        [JsonProperty("gos_id")]
        public string GosId { get; set; }

        [JsonProperty("angle")]
        public int Angle { get; set; }

        [JsonProperty("position")]
        internal List<double> _position {
            set { Position = new LatLon(value); }
        }

        public LatLon Position { get; set; }
    }
}
