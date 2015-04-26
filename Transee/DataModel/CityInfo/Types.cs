using System.Collections.Generic;

namespace Transee.DataModel.CityInfo {
	internal class Types {
		public Types() {
			Items = new List<Type>();
		}

		public Types(List<Type> items) {
			Items = items;
		}

		public Type this[int index] {
			get { return Items[index]; }

			set { Items[index] = value; }
		}

		public List<Type> Items { get; set; }
	}
}