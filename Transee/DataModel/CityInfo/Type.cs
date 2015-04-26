using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;

namespace Transee.DataModel.CityInfo {
	internal class Type {
		private readonly ResourceLoader _resourceLoader = new ResourceLoader();

		[JsonProperty("type")]
		public string Id { get; set; }

		[JsonProperty("items")]
		public List<TypeItem> Items { get; set; }

		public string Name => _resourceLoader.GetString(Id);
	}
}