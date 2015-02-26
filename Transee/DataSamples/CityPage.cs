using System.Collections.Generic;
using Transee.DataModel.CityInfo;

namespace Transee.DataSamples {
    class CityPage {
        public string CityName { get; set; }
        public List<Type> Transports { get; set; }

        public CityPage() {
            var autobus = new List<TypeItem> {
                new TypeItem("1", "1"),
                new TypeItem("2k", "2к"),
                new TypeItem("3", "3"),
                new TypeItem("4", "4"),
                new TypeItem("5", "5"),
                new TypeItem("6", "6"),
                new TypeItem("7", "7"),
                new TypeItem("8", "8"),
                new TypeItem("9", "9"),
                new TypeItem("10", "10"),
                new TypeItem("11", "11"),
                new TypeItem("12", "12"),
                new TypeItem("13", "13"),
                new TypeItem("14", "14"),
                new TypeItem("15", "15"),
            };

            var trolleybus = new List<TypeItem> {
                new TypeItem("1", "1"),
                new TypeItem("2", "2"),
            };

            this.CityName = "ярославль";
            this.Transports = new List<Type> {
                new Type() { Id = "autobus", Items = autobus },
                new Type() { Id = "trolleybus", Items = trolleybus },
            };
        }
    }
}
