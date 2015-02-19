using Transee.Common;
using Transee.DataModel;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Transee.API;
using System.Collections.Generic;

namespace Transee {
    class CityListPageArgs {
        public CityListPageArgs(List<CityItem> cities) {
            this.Cities = cities;
        }

        public List<CityItem> Cities { get; set; }
    }

    public sealed partial class CityListPage : Page {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        Status status = new Status();

        public CityListPage() {
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
            var args = e.NavigationParameter as CityListPageArgs;
            List<CityItem> cities;

            if (args != null) {
                cities = args.Cities;
            } else {
                status.ShowStatusBar("load_cities");
                cities = await CitiesFetcher.GetAsync();
                status.HideStatusBar();
            }

            this.DefaultViewModel["Cities"] = cities;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
            // TODO: Сохраните здесь уникальное состояние страницы.
        }

        private async void ItemView_ItemClick(object sender, ItemClickEventArgs e) {
            status.ShowStatusBar("load_city_data");

            var cityId = ((CityItem) e.ClickedItem).Id;
            // load data, request should cache it
            var stations = await StationsFetcher.GetAsync(cityId);
            var routes = await RoutesFetcher.GetAsync(cityId);
            var transports = await TransportsFetcher.GetAsync(cityId);

            settings.Values["CityID"] = cityId;

            status.HideStatusBar();

            if (!Frame.Navigate(typeof(CityPage), cityId)) {
                var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
                throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
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
