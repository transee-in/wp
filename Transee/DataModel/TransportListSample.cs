using System.Collections.Generic;

namespace Transee.DataModel {
    public class TransportListSample {
        public string CityName { get; set; }
        public List<TransportType> Transports { get; set; }

        public TransportListSample() {
            var autobus = new List<TransportItem> {
                new TransportItem("1", "1"),
                new TransportItem("2k", "2к"),
                new TransportItem("3", "3"),
                new TransportItem("4", "4"),
                new TransportItem("5", "5"),
                new TransportItem("6", "6"),
                new TransportItem("7", "7"),
                new TransportItem("8", "8"),
                new TransportItem("9", "9"),
                new TransportItem("10", "10"),
                new TransportItem("11", "11"),
                new TransportItem("12", "12"),
                new TransportItem("13", "13"),
                new TransportItem("14", "14"),
                new TransportItem("15", "15"),
            };

            var trolleybus = new List<TransportItem> {
                new TransportItem("1", "1"),
                new TransportItem("2", "2"),
            };

            this.CityName = "ярославль";
            this.Transports = new List<TransportType> {
                new TransportType("autobus", autobus),
                new TransportType("trolleybus", trolleybus),
            };
        }
    }
}
