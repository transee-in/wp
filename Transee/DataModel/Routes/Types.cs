using System.Collections.Generic;

namespace Transee.DataModel.Routes {
    class Types {
        public List<Type> Items { get; set; }

        public Types() {
            Items = new List<Type>();
        }

        public Types(List<Type> items) {
            Items = items;
        }

        public Type GetTypeByName(string name) {
            Type foundedItem = null;

            foreach (var item in Items) {
	            if (item.Name != name) continue;
	            foundedItem = item;
	            break;
            }

            return foundedItem;
        }

        public Route GetRouteByTypeNameAndRouteId(string typeName, string routeID) {
	        var type = GetTypeByName(typeName);

	        return type?.GetRouteById(routeID);
        }
    }
}
