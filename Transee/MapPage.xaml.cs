using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Transee.API;
using Transee.Common;
using Transee.DataModel;
using Transee.DataModel.Positions;

namespace Transee {
	internal class MapPageArgs {
		public MapPageArgs(string cityId, Dictionary<string, List<string>> types) {
			CityId = cityId;
			Types = types;
		}

		public string CityId { get; set; }
		public Dictionary<string, List<string>> Types { get; set; }
	}

	internal class MapInfoContentItem {
		public string Text { get; }
		public string Info { get; }

		public MapInfoContentItem(string text, string info) {
			Text = text;
			Info = info;
		}
	}

	internal class MapInfoContent {
		public List<MapInfoContentItem> Items { get; } = new List<MapInfoContentItem>();

		public void Clear() {
			Items.Clear();
		}
	}

	public sealed partial class MapPage {
		public NavigationHelper NavigationHelper { get; }
		public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();

		private MapPageArgs _mapParams;
		private readonly MapInfoContent _mapInfoContent = new MapInfoContent();
		private readonly DispatcherTimer _timer = new DispatcherTimer();
		private readonly MarkerColors _markerColors = MarkerColors.GetDefaultMarkerColors();

		private Types _positions;
		private DataModel.Routes.Types _routes;
		private LatLon _coordinates;

		public MapPage() {
			InitializeComponent();

			NavigationHelper = new NavigationHelper(this);
			NavigationHelper.LoadState += NavigationHelper_LoadState;
			NavigationHelper.SaveState += NavigationHelper_SaveState;
		}

		private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) {
			_mapParams = e.NavigationParameter as MapPageArgs;
			if (_mapParams == null) {
				return;
			}

			DefaultViewModel["MarkerColors"] = _markerColors;
			DefaultViewModel["MapInfoContent"] = _mapInfoContent;

			// load city coordinates
			_coordinates = await CoordinatesFetcher.GetAsync(_mapParams.CityId);

			// navigate user to city
			map.Center = new Geopoint(new BasicGeoposition {
				Latitude = _coordinates.Lat,
				Longitude = _coordinates.Lon
			});
			map.ZoomLevel = 12;
			// map.HeadingChanged += Map_HeadingChanged;

			// load city data
			_positions = await PositionsFetcher.GetAsync(_mapParams.CityId, _mapParams.Types);
			_routes = await RoutesFetcher.GetAsync(_mapParams.CityId);

			DrawRoutes();
			DrawPositions();

			// start city positions reloader
			_timer.Tick += ReloadAndRedrawPositions_Tick;
			_timer.Interval = new TimeSpan(0, 0, 10);
			_timer.Start();
		}

		private void Map_HeadingChanged(MapControl sender, object args) {
			// throw new NotImplementedException();
			// TODO change markers angle
		}

		private void DrawRoutes() {
			foreach (var type in _positions.Items) {
				foreach (var typeItem in type.Items) {
					foreach (var item in typeItem.Items) {
						var route = _routes.GetRouteByTypeNameAndRouteId(type.Name, typeItem.Id);
						if (route != null) {
							var line = route.CreateMapPolyline(type.Name);
							map.MapElements.Add(line);
						}
					}
				}
			}
		}

		private void DrawPositions() {
			foreach (var type in _positions.Items) {
				foreach (var typeItem in type.Items) {
					foreach (var item in typeItem.Items) {
						var point = new Geopoint(new BasicGeoposition() {
							Latitude = item.Position.Lat,
							Longitude = item.Position.Lon,
						});

						var tpl = Resources["pinTemplate"] as DataTemplate;
						if (tpl == null) {
							continue;
						}
						var cnt = tpl.LoadContent() as UIElement;
						var text = FindVisualChild<TextBlock>(cnt);
						var elps = FindVisualChild<Ellipse>(cnt);
						var plgn = FindVisualChild<Polygon>(cnt);

						text.Text = typeItem.Name;
						elps.Fill = new SolidColorBrush(_markerColors.GetColorByName(type.Name));
						plgn.Fill = new SolidColorBrush(_markerColors.GetColorByName(type.Name));

						RotateMarker(item.Angle, cnt, text);
						AddToMap(cnt, point);
						// AttachClickEvent(cnt, type.Name, item);
					}
				}
			}
		}

		private async void ReloadAndRedrawPositions_Tick(object sender, object e) {
			_positions = await PositionsFetcher.GetAsync(_mapParams.CityId, _mapParams.Types);

			await Dispatcher.RunAsync(CoreDispatcherPriority.High, () => {
				CleanMap();
				DrawPositions();
			});
		}

		private static TChildItem FindVisualChild<TChildItem>(DependencyObject obj) where TChildItem : DependencyObject {
			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {
				var child = VisualTreeHelper.GetChild(obj, i);

				var item = child as TChildItem;
				if (item != null) {
					return item;
				}

				var childOfChild = FindVisualChild<TChildItem>(child);
				if (childOfChild != null) {
					return childOfChild;
				}
			}
			return null;
		}

		private static void RotateMarker(double angle, UIElement el, TextBlock text) {
			el.RenderTransform = new CompositeTransform() {
				ScaleX = 0.3,
				ScaleY = 0.3,
				Rotation = angle
			};

			text.RenderTransform = new RotateTransform() {
				Angle = -angle
			};
		}

		private void AttachClickEvent(UIElement obj, string typeName, Item item) {
			obj.Tapped += async (sender, args) => {
				_mapInfoContent.Clear();

				System.Diagnostics.Debug.WriteLine("tap {0} {1}", typeName, item.GosId);
				// TODO show progress
				var transportInfo = await TrasportInfoFetcher.GetAsync(_mapParams.CityId, typeName, item.GosId);
				foreach (var transportInfoItem in transportInfo.Items) {
					//System.Diagnostics.Debug.WriteLine("info: {0} {1}", transportInfoItem.Name, transportInfoItem.Time);

					_mapInfoContent.Items.Add(new MapInfoContentItem(transportInfoItem.Name, transportInfoItem.Time));
                }

				mapInfo.Visibility = Visibility.Visible;
			};
		}

		private void AddToMap(DependencyObject obj, Geopoint point) {
			MapControl.SetLocation(obj, point);
			MapControl.SetNormalizedAnchorPoint(obj, new Point(0.5, 0.5));
			map.Children.Add(obj);
		}

		private void CleanMap() {
			map.Children.Clear();
		}

		private void legendButton_Tapped(object sender, TappedRoutedEventArgs e) {
			mapInfo.Visibility = Visibility.Collapsed;
		}

		private void Canvas_Tapped(object sender, TappedRoutedEventArgs e) {
			// ...
		}

		private static void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
			// TODO: Сохраните здесь уникальное состояние страницы.
		}

		#region NavigationHelper

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			NavigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e) {
			_timer.Stop();
			NavigationHelper.OnNavigatedFrom(e);
		}

		#endregion
	}
}