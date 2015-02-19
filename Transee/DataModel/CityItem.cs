using System;
using Windows.ApplicationModel.Resources;

namespace Transee.DataModel {
    public class CityItem {
        private ResourceLoader resourceLoader = new ResourceLoader();

        public CityItem(String id) {
            this.Id = id;
        }

        public string Id { get; private set; }
        public string Name {
            get {
                return resourceLoader.GetString(Id);
            }
        }

        public override string ToString() {
            return this.Name;
        }
    }
}
