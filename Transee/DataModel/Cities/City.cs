using Windows.ApplicationModel.Resources;

namespace Transee.DataModel.Cities {
    public class City {
	    private readonly ResourceLoader _resourceLoader = new ResourceLoader();

        public City(string id) {
            Id = id;
        }

        public string Id { get; }

	    public string Name => _resourceLoader.GetString(Id);

	    public static implicit operator City(string id) => id == null ? null : new City(id);
    }
}
