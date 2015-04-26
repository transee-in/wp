namespace Transee.DataSamples {
	internal class MapPage {
		public DataModel.MarkerColors MarkerColors;
		public MapInfoContent MapInfoContent;

		public MapPage() {
			MarkerColors = DataModel.MarkerColors.GetDefaultMarkerColors();
			MapInfoContent = new MapInfoContent();

			MapInfoContent.Items.Add(new MapInfoContentItem("test", "12:32"));
		}
	}
}