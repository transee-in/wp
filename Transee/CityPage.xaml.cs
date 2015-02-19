using Transee.Common;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using Transee.DataModel;
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
        private Dictionary<string, TransportType> selectedTransports = new Dictionary<string, TransportType>();
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
            var transports = await TransportsFetcher.GetAsync(this.CityId);

            this.DefaultViewModel["CityName"] = cityName;
            this.DefaultViewModel["Transports"] = transports;
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
                foreach (var element in type.Value.Elements) {
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
            var transportTypeID = listBox.DataContext.ToString();
            var transportType = new TransportType(transportTypeID);

            foreach (TransportItem item in listBox.SelectedItems) {
                transportType.Elements.Add(item);
            }

            if (selectedTransports.ContainsKey(transportTypeID)) {
                selectedTransports.Remove(transportTypeID);
            }

            selectedTransports.Add(transportTypeID, transportType);
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
