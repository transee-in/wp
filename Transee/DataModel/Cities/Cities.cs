using System.Collections.Generic;

namespace Transee.DataModel.Cities {
    class Cities {
        public List<City> Items { get; set; }

        public Cities() {
            this.Items = new List<City>();
        }

        public Cities(List<City> items) {
            this.Items = items;
        }
    }
}
