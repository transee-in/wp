using System.Collections.Generic;

namespace Transee.DataModel {
    class PositionType {
        public PositionType(string type) {
            this.Type = type;
            this.Items = new Dictionary<string, PositionItem>();
        }

        public string Type { get; set; }
        public Dictionary<string, PositionItem> Items { get; set; }
    }
}
