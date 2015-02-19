using System.Collections.Generic;

namespace Transee.DataModel {
    class RouteType {
        public RouteType(string type) {
            this.Type = type;
            this.Items = new Dictionary<string, RouteItem>();
        }

        public string Type { get; set; }
        public Dictionary<string, RouteItem> Items { get; set; }
    }
}
