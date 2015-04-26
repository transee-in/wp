using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace Transee.DataModel {
	internal class MarkerColor {
		public MarkerColor(string name, Windows.UI.Color color) {
			Name = name;
			Color = color;
		}

		public string Name { get; set; }
		public Windows.UI.Color Color { get; set; }

		public SolidColorBrush Brush => new SolidColorBrush(Color);
	}

	internal class MarkerColors {
		public List<MarkerColor> Colors { get; set; }

		public Windows.UI.Color GetColorByName(string name) {
			return Colors.First(x => x.Name == name).Color;
		}

		public static MarkerColors GetDefaultMarkerColors() {
			return new MarkerColors {
				Colors = new List<MarkerColor> {
					new MarkerColor("autobus", Windows.UI.Colors.Green),
					new MarkerColor("trolleybus", Windows.UI.Colors.Indigo),
					new MarkerColor("tram", Windows.UI.Colors.Orange),
					new MarkerColor("minibus_taxi", Windows.UI.Colors.Maroon),
				}
			};
		}
	}
}