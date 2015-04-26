using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transee.DataModel.Stations;

namespace Transee.API {
	internal class StationsFetcher {
		public static async Task<Types> GetAsync(string city) {
			var request = new Request();
			var jsonData = await request.GetCity(city, "stations");
			var listStations = JsonConvert.DeserializeObject<List<Type>>(jsonData);

			return new Types(listStations);
		}
	}
}