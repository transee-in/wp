using System.Collections.Generic;

namespace Transee.DataModel.Routes {
    class Types {
        public List<Type> Items { get; set; }

        public Types() {
            this.Items = new List<Type>();
        }

        public Types(List<Type> items) {
            this.Items = items;
        }

        public Type GetTypeByName(string name) {
            Type foundedItem = null;

            foreach (var item in Items) {
                if (item.Name == name) {
                    foundedItem = item;
                    break;
                }
            }

            return foundedItem;
        }

        public Route GetRouteByTypeNameAndRouteId(string typeName, string routeID) {
            var type = this.GetTypeByName(typeName);

            if (type != null) {
                return type.GetRouteById(routeID);
            } else {
                return null;
            }
        }
    }
}
