using Transee.Common;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using Transee.DataModel.CityInfo;
using System;
using Transee.API;
using Windows.Storage;

namespace Transee {
    public sealed partial class CityPage : Page {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        private ResourceLoader resourceLoader = new ResourceLoader();
        private Status status = new Status();
        private Dictionary<string, DataModel.CityInfo.Type> selectedTransports = new Dictionary<string, DataModel.CityInfo.Type>();
        private string CityId;

        public CityPage() {
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
            this.CityId = e.NavigationParameter.ToString();
            var cityName = resourceLoader.GetString(this.CityId);
            var transports = await CityInfoFetcher.GetAsync(this.CityId);

            // TODO: add page with favorite transports
            this.DefaultViewModel["CityName"] = cityName;
            this.DefaultViewModel["Transports"] = transports.Items;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
        }

        private void AppBarButton_ClickCities(object sender, RoutedEventArgs e) {
            if (!Frame.Navigate(typeof(CityListPage))) {
                var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
                throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void AppBarButton_ClickNext(object sender, RoutedEventArgs e) {
            var types = new Dictionary<string, List<string>>();

            status.ShowStatusBar("load_positions");

            foreach (var type in selectedTransports) {
                var transportIds = new List<string>();
                foreach (var element in type.Value.Items) {
                    transportIds.Add(element.Id);
                }
                types.Add(type.Key, transportIds);
            }

            // var positions = await PositionsFetcher.GetAsync(this.CityId, types);
            var positionTypes = new MapPageArgs(this.CityId, types);

            status.HideStatusBar();

            if (!Frame.Navigate(typeof(MapPage), positionTypes)) {
                var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
                throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var listBox = sender as ListBox;
            var cityInfoType = listBox.DataContext as DataModel.CityInfo.Type;
            var transportTypeId = cityInfoType.Id;
            //var transportType = new DataModel.CityInfo.Type(transportTypeId);

            var transportType = new DataModel.CityInfo.Type() {
                Id = transportTypeId, Items = new List<TypeItem>()
            };

            foreach (TypeItem item in listBox.SelectedItems) {
                transportType.Items.Add(item);
            }

            if (selectedTransports.ContainsKey(transportTypeId)) {
                selectedTransports.Remove(transportTypeId);
            }

            selectedTransports.Add(transportTypeId, transportType);
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
