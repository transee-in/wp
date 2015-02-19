using System.Collections.Generic;

namespace Transee.DataModel {
    class PositionItem {
        public PositionItem() {
            this.Route = new List<LatLonAz>();
        }

        public List<LatLonAz> Route { get; set; }
    }
}
