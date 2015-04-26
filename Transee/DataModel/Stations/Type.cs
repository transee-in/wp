using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Stations {
	internal class Type {
		[JsonProperty("type")]
		public string Name { get; set; }

		[JsonProperty("items")]
		public List<Item> Items { get; set; }
	}
}