using System.Collections.Generic;

namespace Transee.DataModel.Positions {
	internal class Types {
		public Types() {
			Items = new List<Type>();
		}

		public Types(List<Type> items) {
			Items = items;
		}

		public List<Type> Items { get; set; }
	}
}