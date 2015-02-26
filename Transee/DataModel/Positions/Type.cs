﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Positions {
    class Type {
        [JsonProperty("type")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<TypeItem> Items { get; set; }
    }
}
