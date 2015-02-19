using System.Collections.Generic;

namespace Transee.DataModel {
    class LatLon {
        public LatLon(List<double> rawLatLon) {
            this.Lat = rawLatLon[0];
            this.Lon = rawLatLon[1];
        }

        public LatLon(double lat, double lon) {
            this.Lat = lat;
            this.Lon = lon;
        }

        public LatLon() { }

        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
