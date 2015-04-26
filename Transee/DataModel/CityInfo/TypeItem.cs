using Newtonsoft.Json;

namespace Transee.DataModel.CityInfo {
	internal class TypeItem {
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		public TypeItem(string id, string name) {
			Id = id;
			Name = name;
		}
	}
}