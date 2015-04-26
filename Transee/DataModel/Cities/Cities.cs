using System.Collections.Generic;

namespace Transee.DataModel.Cities {
	internal class Cities {
		public List<City> Items { get; set; }

		public Cities() {
			Items = new List<City>();
		}

		public Cities(List<City> items) {
			Items = items;
		}
	}
}