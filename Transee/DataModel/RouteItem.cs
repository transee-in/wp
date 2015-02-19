using System.Collections.Generic;

namespace Transee.DataModel {
    class RouteItem {
        public RouteItem() {
            this.Route = new List<LatLon>();
        }

        public List<LatLon> Route { get; set; }
    }
}
