using System.Collections.Generic;

namespace Transee.DataModel.CityInfo {
    class Types {
        public Types() {
            this.Items = new List<Type>();
        }

        public Types(List<Type> items) {
            this.Items = items;
        }

        public Type this[int index] {
            get {
                return this.Items[index];
            }

            set {
                this.Items[index] = value;
            }
        }

        public List<Type> Items { get; set; }
    }
}
