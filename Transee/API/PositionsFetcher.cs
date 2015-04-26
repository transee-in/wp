using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Transee.DataModel.Positions;

namespace Transee.API {
	using Filter = Dictionary<string, List<string>>;

	internal class PositionsFetcher {
		public static async Task<Types> GetAsync(string city, Filter filter) {
			var request = new Request();
			var jsonData = await request.Post(city, "positions", PrepareFilter(filter));
			var listPositions = JsonConvert.DeserializeObject<List<Type>>(jsonData);

			return new Types(listPositions);
		}

		private static FormUrlEncodedContent PrepareFilter(Filter filter) {
			var qs = new List<KeyValuePair<string, string>>();

			foreach (var numbers in filter) {
				qs.Add(new KeyValuePair<string, string>("type[]", numbers.Key));

				foreach (var number in numbers.Value) {
					var key = $"numbers[{numbers.Key}][]";
					qs.Add(new KeyValuePair<string, string>(key, number));
				}
			}

			return new FormUrlEncodedContent(qs);
		}
	}
}