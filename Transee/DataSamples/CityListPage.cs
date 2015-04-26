using System.Collections.Generic;
using Transee.DataModel.Cities;

namespace Transee.DataSamples {
	internal class CityListPage {
		public List<City> Cities { get; set; }

		public CityListPage() {
			Cities = new List<City> {
				new City("yaroslavl"),
				new City("novosibirsk"),
				new City("moscow"),
			};
		}
	}
}