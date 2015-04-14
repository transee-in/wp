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
            Cities = cities;
        }

        public Cities Cities { get; set; }
    }

    public sealed partial class CityListPage : Page {
        private readonly NavigationHelper _navigationHelper;
        private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();
        private readonly ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;
	    private readonly Status _status = new Status();

        public CityListPage() {
            InitializeComponent();

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
            _navigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper => _navigationHelper;

	    public ObservableDictionary DefaultViewModel => _defaultViewModel;

	    private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) {
            var args = e.NavigationParameter as CityListPageArgs;
            Cities cities;

            if (args != null) {
                cities = args.Cities;
            } else {
                _status.ShowStatusBar("load_cities");
                cities = await CitiesFetcher.GetAsync();
                _status.HideStatusBar();
            }

            DefaultViewModel["Cities"] = cities.Items;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
            // TODO: Сохраните здесь уникальное состояние страницы.
        }

        private async void ItemView_ItemClick(object sender, ItemClickEventArgs e) {
            _status.ShowStatusBar("load_city_data");

            var cityId = ((City) e.ClickedItem).Id;
            // load data, Request class should cache it
            await StationsFetcher.GetAsync(cityId);
            await RoutesFetcher.GetAsync(cityId);
            await CityInfoFetcher.GetAsync(cityId);
            await CoordinatesFetcher.GetAsync(cityId);

            _settings.Values["CityID"] = cityId;

            _status.HideStatusBar();

            if (!Frame.Navigate(typeof(CityPage), cityId)) {
                var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
                throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        #region NavigationHelper

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
