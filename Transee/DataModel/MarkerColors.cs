using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace Transee.DataModel {
    class MarkerColor {
        public MarkerColor(string name, Windows.UI.Color color) {
            this.Name = name;
            this.Color = color;
        }

        public string Name { get; set; }
        public Windows.UI.Color Color { get; set; }

        public SolidColorBrush Brush {
            get {
                return new SolidColorBrush(this.Color);
            }
        }
    }

    class MarkerColors {
        public List<MarkerColor> Colors { get; set; }

        public Windows.UI.Color GetColorByName(string name) {
            return Colors.Where((x) => x.Name == name).First().Color;
        }

        public static MarkerColors GetDefaultMarkerColors() {
            return new MarkerColors() {
                Colors = new List<MarkerColor>() {
                    new MarkerColor("autobus", Windows.UI.Colors.Green),
                    new MarkerColor("trolleybus", Windows.UI.Colors.Indigo),
                    new MarkerColor("tram", Windows.UI.Colors.Orange),
                    new MarkerColor("minibus_taxi", Windows.UI.Colors.Maroon),
                }
            };
        }
    }
}
