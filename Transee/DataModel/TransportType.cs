using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;

namespace Transee.DataModel {
    public class TransportType {
        private ResourceLoader resourceLoader = new ResourceLoader();

        public TransportType(String type) {
            this.Type = type;
            this.Elements = new List<TransportItem>();
        }

        public TransportType(String type, List<TransportItem> elements) {
            this.Type = type;
            this.Elements = elements;
        }

        public string Type { get; set; }
        public List<TransportItem> Elements { get; set; }
        public string Name {
            get {
                return resourceLoader.GetString(Type);
            }
        }
        public override string ToString() {
            return this.Type;
        }
    }
}
