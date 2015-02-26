using System.Collections.Generic;

namespace Transee.DataModel.Positions {
    class Types {
        public Types() {
            this.Items = new List<Type>();
        }

        public Types(List<Type> items) {
            this.Items = items;
        }

        public List<Type> Items { get; set; }
    }
}
