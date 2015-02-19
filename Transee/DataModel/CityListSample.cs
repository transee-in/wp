using System.Collections.Generic;

namespace Transee.DataModel {
    public class CityListSample {
        public List<CityItem> Cities { get; set; }

        public CityListSample() {
            this.Cities = Load();
        }

        List<CityItem> Load() {
            return new List<CityItem> {
                new CityItem("yaroslavl"),
                new CityItem("novosibirsk"),
                new CityItem("moscow"),
            };
        }
    }
}
