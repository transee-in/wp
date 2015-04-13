using System.Collections.Generic;
using Transee.DataModel.Cities;

namespace Transee.DataSamples {
    class CityListPage {
        public Cities Cities { get; set; }

        public CityListPage() {
            Cities = Load();
        }

        public Cities Load() {
            return new Cities {
                Items = new List<City> {
                    new City("yaroslavl"),
                    new City("novosibirsk"),
                    new City("moscow"),
                }
            };
        }
    }
}
