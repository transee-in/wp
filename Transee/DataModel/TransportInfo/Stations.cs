using System.Collections.Generic;

namespace Transee.DataModel.TransportInfo {
	internal class Stations {
		public List<Station> Items { get; set; }

		public Stations() {
			Items = new List<Station>();
		}

		public Stations(List<Station> items) {
			Items = items;
		}
	}
}
