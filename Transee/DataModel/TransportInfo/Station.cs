using Newtonsoft.Json;

namespace Transee.DataModel.TransportInfo {
	internal class Station {
		[JsonProperty("station")]
		public string Name { get; set; }

		[JsonProperty("time")]
		public string Time { get; set; }
	}
}
