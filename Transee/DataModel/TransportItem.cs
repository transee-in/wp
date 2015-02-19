using System;

namespace Transee.DataModel {
    public class TransportItem {
        public TransportItem(String id, String name) {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public override string ToString() {
            return this.Name;
        }
    }
}
