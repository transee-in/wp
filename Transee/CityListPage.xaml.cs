using Transee.Common;
using Transee.DataModel.Cities;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Transee.API;

namespace Transee {
    class CityListPageArgs {
        public CityListPageArgs(Cities cities) {
            this.Cities = cities;
        }

        public Cities Cities { get; set; }
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
            Cities cities;

            if (args != null) {
                cities = args.Cities;
            } else {
                status.ShowStatusBar("load_cities");
                cities = await CitiesFetcher.GetAsync();
                status.HideStatusBar();
            }

            this.DefaultViewModel["Cities"] = cities.Items;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
            // TODO: Сохраните здесь уникальное состояние страницы.
        }

        private async void ItemView_ItemClick(object sender, ItemClickEventArgs e) {
            status.ShowStatusBar("load_city_data");

            var cityId = ((City) e.ClickedItem).Id;
            // load data, Request class should cache it
            var stations = await StationsFetcher.GetAsync(cityId);
            var routes = await RoutesFetcher.GetAsync(cityId);
            var transports = await CityInfoFetcher.GetAsync(cityId);

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
