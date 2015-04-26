using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel;

namespace Transee.API {
	internal class CoordinatesFetcher {
		public static async Task<LatLon> GetAsync(string city) {
			var request = new Request();
			var jsonData = await request.GetCity(city, "coordinates");
			var coordinates = JsonConvert.DeserializeObject<List<double>>(jsonData);

			return new LatLon(coordinates);
		}
	}
}