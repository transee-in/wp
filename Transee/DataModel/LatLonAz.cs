using System.Collections.Generic;

namespace Transee.DataModel {
    class LatLonAz {
        public LatLonAz(List<double> rawLatLonAz) {
            this.Lat = rawLatLonAz[0];
            this.Lon = rawLatLonAz[1];
            this.Az = (int) rawLatLonAz[2];
        }

        public LatLonAz(double lat, double lon, int az) {
            this.Lat = lat;
            this.Lon = lon;
            this.Az = az;
        }

        public LatLonAz() { }

        public double Lat { get; set; }
        public double Lon { get; set; }
        public int Az { get; set; }
    }
}
