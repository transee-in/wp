using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transee.DataModel.Stations {
	internal class Item {
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("position")]
		internal virtual List<double> _position {
			set { Position = new LatLon(value); }
		}

		public LatLon Position { get; set; }
	}
}