using System;
using System.Collections.Generic;
using System.Threading;
using Transee.API;
using Transee.Common;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
        private MapPageArgs mapParams;
        private DispatcherTimer timer = new DispatcherTimer();

        // colors for markers
        private Dictionary<string, Color> markerColors = new Dictionary<string, Color>() {
            { "autobus", Colors.Green },
            { "trolleybus", Colors.Indigo },
            { "tram", Colors.Orange }
        };

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

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) {
            this.mapParams = e.NavigationParameter as MapPageArgs;

            // navigate user to yaroslavl
            map.Center = new Geopoint(new BasicGeoposition() {
                Latitude = 57.6166667,
                Longitude = 39.8666667
            });
            map.ZoomLevel = 12;
            map.LandmarksVisible = true;

            // load city data
            // var allRoutes = await RoutesFetcher.GetAsync(args.CityId);
            // var allStations = await StationsFetcher.GetAsync(args.CityId);

            var t = new Timer(TimerCallback, this, 0, 30 * 1000);
        }

        private static async void TimerCallback(object state) {
            var context = state as MapPage;
            var positions = await PositionsFetcher.GetAsync(context.mapParams.CityId, context.mapParams.Types);

            await context.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => {
                context.CleanMap();
            });

            foreach (var position in positions) {
                foreach (var item in position.Items) {
                    foreach (var latLonAz in item.Value.Route) {
                        var point = new Geopoint(new BasicGeoposition() {
                            Latitude = latLonAz.Lat,
                            Longitude = latLonAz.Lon
                        });

                        await context.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => {
                            var tpl = context.Resources["pinTemplate"] as DataTemplate;
                            var cnt = tpl.LoadContent() as UIElement;
                            var text = context.FindVisualChild<TextBlock>(cnt);
                            var path = context.FindVisualChild<Path>(cnt);

                            text.Text = item.Key;
                            path.Fill = new SolidColorBrush(context.markerColors[position.Type]);

                            context.RotateMarker(latLonAz.Az, cnt, text);
                            context.AddToMap(cnt, point);
                        });
                    }
                }
            }
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
            map.Children.Add(obj);
            MapControl.SetLocation(obj, point);
            MapControl.SetNormalizedAnchorPoint(obj, new Point(0.5, 0.5));
        }

        private void CleanMap() {
            map.Children.Clear();
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
            // TODO: Сохраните здесь уникальное состояние страницы.
        }

        #region Регистрация NavigationHelper

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
