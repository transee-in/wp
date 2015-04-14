using System.Collections.Generic;

namespace Transee.DataModel {
    class LatLon {
        public LatLon(IReadOnlyList<double> rawLatLon) {
            Lat = rawLatLon[0];
            Lon = rawLatLon[1];
        }

        public LatLon(double lat, double lon) {
            Lat = lat;
            Lon = lon;
        }

        public LatLon() { }

        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
