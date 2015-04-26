using System.Collections.Generic;
using System.Linq;

namespace Transee.DataModel.Routes {
	internal class Types {
		public List<Type> Items { get; set; }

		public Types() {
			Items = new List<Type>();
		}

		public Types(List<Type> items) {
			Items = items;
		}

		public Type GetTypeByName(string name) => Items.FirstOrDefault(item => item.Name == name);

		public Route GetRouteByTypeNameAndRouteId(string typeName, string routeId) {
			var type = GetTypeByName(typeName);

			return type?.GetRouteById(routeId);
		}
	}
}