using Windows.ApplicationModel.Resources;

namespace Transee.DataModel.Cities {
    public class City {
        private string _id;
        private ResourceLoader resourceLoader = new ResourceLoader();

        public City(string id) {
            this._id = id;
        }

        public string Id {
            get { return this._id; }
        }

        public string Name {
            get { return resourceLoader.GetString(this._id); }
        }

        public static implicit operator City(string id) {
            if (id == null) {
                return null;
            } else {
                return new City(id);
            }
        }
    }
}
