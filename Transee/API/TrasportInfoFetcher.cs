using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Transee.DataModel.TransportInfo;

namespace Transee.API {
	internal class TrasportInfoFetcher {
		public static async Task<Stations> GetAsync(string city, string typeName, string gosId) {
			var request = new Request();
			var jsonData = await request.Post(city, "transport_info", PrepareFilter(typeName, gosId));
			var listStations = JsonConvert.DeserializeObject<List<Station>>(jsonData);

			return new Stations(listStations);
		}

		private static FormUrlEncodedContent PrepareFilter(string typeName, string gosId) {
			var qs = new List<KeyValuePair<string, string>> {
				new KeyValuePair<string, string>("type", typeName),
				new KeyValuePair<string, string>("gos_id", gosId)
			};

			return new FormUrlEncodedContent(qs);
		}
	}
}