using System;
using System.Collections.Generic;
using System.Diagnostics;
using Transee.API;
using Transee.Common;
using Transee.DataModel;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace Transee {
    class MapPageArgs {
        public MapPageArgs(string cityId, Dictionary<string, List<string>> types) {
            this.CityId = cityId;
            this.Types = types;
        }

        public string CityId { get; set; }
        public Dictionary<string, List<string>> Types { get; set; }
    }

    public sealed partial class MapPage : Page {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        private ResourceLoader resourceLoader = new ResourceLoader();
        private MapPageArgs mapParams;
        private DispatcherTimer timer = new DispatcherTimer();
        private MarkerColors markerColors = MarkerColors.GetDefaultMarkerColors();

        private DataModel.Positions.Types positions;
        private DataModel.Routes.Types routes;
        private LatLon coordinates;

        public MapPage() {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel {
            get { return this.defaultViewModel; }
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) {
            this.mapParams = e.NavigationParameter as MapPageArgs;

            this.DefaultViewModel["MarkerColors"] = this.markerColors;

            // load city coordinates
            this.coordinates = await CoordinatesFetcher.GetAsync(this.mapParams.CityId);

            // navigate user to city
            this.map.Center = new Geopoint(new BasicGeoposition() {
                Latitude = this.coordinates.Lat,
                Longitude = this.coordinates.Lon
            });
            this.map.ZoomLevel = 12;
            // map.HeadingChanged += Map_HeadingChanged;

            // load city data
            this.positions = await PositionsFetcher.GetAsync(this.mapParams.CityId, this.mapParams.Types);
            this.routes = await RoutesFetcher.GetAsync(this.mapParams.CityId);

            this.DrawRoutes();
            this.DrawPositions();

            // start city positions reloader
            this.timer.Tick += this.ReloadAndRedrawPositions_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 10);
            this.timer.Start();
        }

        private void Map_HeadingChanged(MapControl sender, object args) {
            // throw new NotImplementedException();
            // TODO change markers angle
        }

        private void DrawRoutes() {
            foreach (var type in positions.Items) {
                foreach (var typeItem in type.Items) {
                    foreach (var item in typeItem.Items) {
                        var route = routes.GetRouteByTypeNameAndRouteId(type.Name, typeItem.Id);
                        if (route != null) {
                            var line = route.CreateMapPolyline();
                            line.StrokeColor = this.GetRandomColor();
                            line.StrokeThickness = 5;

                            this.map.MapElements.Add(line);
                        }
                    }
                }
            }
        }

        private Windows.UI.Color GetRandomColor() {
            return new Windows.UI.Color() { A = 100, R = 3, G = 166, B = 120 };
        }

        private void DrawPositions() {
            foreach (var type in positions.Items) {
                foreach (var typeItem in type.Items) {
                    foreach (var item in typeItem.Items) {
                        var point = new Geopoint(new BasicGeoposition() {
                            Latitude = item.Position.Lat,
                            Longitude = item.Position.Lon,
                        });

                        var tpl = this.Resources["pinTemplate"] as DataTemplate;
                        var cnt = tpl.LoadContent() as UIElement;
                        var text = this.FindVisualChild<TextBlock>(cnt);
                        var elps = this.FindVisualChild<Ellipse>(cnt);
                        var plgn = this.FindVisualChild<Polygon>(cnt);

                        text.Text = typeItem.Name;
                        elps.Fill = new SolidColorBrush(this.markerColors.GetColorByName(type.Name));
                        plgn.Fill = new SolidColorBrush(this.markerColors.GetColorByName(type.Name));

                        this.RotateMarker(item.Angle, cnt, text);
                        this.AddToMap(cnt, point);
                    }
                }
            }
        }

        private async void ReloadAndRedrawPositions_Tick(object sender, object e) {
            positions = await PositionsFetcher.GetAsync(this.mapParams.CityId, this.mapParams.Types);

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => {
                this.CleanMap();
                this.DrawPositions();
            });
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childItem) {
                    return (childItem) child;
                } else {
                    childItem childOfChild = this.FindVisualChild<childItem>(child);
                    if (childOfChild != null) {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        private void RotateMarker(double angle, UIElement el, TextBlock text) {
            el.RenderTransform = new CompositeTransform() {
                ScaleX = 0.3, ScaleY = 0.3,
                Rotation = angle
            };

            text.RenderTransform = new RotateTransform() {
                Angle = -angle
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
            if (legendBlock.Visibility == Visibility.Collapsed) {
                legendBlock.Visibility = Visibility.Visible;
            } else {
                legendBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
            // TODO: Сохраните здесь уникальное состояние страницы.
        }

        #region Регистрация NavigationHelper

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            timer.Stop();
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Canvas_Tapped(object sender, TappedRoutedEventArgs e) {
            // ...
        }
    }
}
