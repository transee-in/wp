using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;

namespace Transee.DataModel {
    class StationType {
        private ResourceLoader resourceLoader = new ResourceLoader();

        public StationType(String type) {
            this.Type = type;
            this.Items = new List<LatLon>();
        }

        public StationType(String type, List<LatLon> items) {
            this.Type = type;
            this.Items = items;
        }

        public string Type { get; set; }
        public List<LatLon> Items { get; set; }

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
