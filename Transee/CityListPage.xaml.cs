using Transee.Common;
using Transee.DataModel.Cities;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Transee.API;

namespace Transee {
	internal class CityListPageArgs {
		public CityListPageArgs(Cities cities) {
			Cities = cities;
		}

		public Cities Cities { get; set; }
	}

	public sealed partial class CityListPage {
		public NavigationHelper NavigationHelper { get; }
		public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();

		private readonly ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;
		private readonly Status _status = new Status();

		public CityListPage() {
			InitializeComponent();

			NavigationHelper = new NavigationHelper(this);
			NavigationHelper.LoadState += NavigationHelper_LoadState;
			NavigationHelper.SaveState += NavigationHelper_SaveState;
		}

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

		private static void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
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

			if (!Frame.Navigate(typeof (CityPage), cityId)) {
				var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
				throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
			}
		}

		#region NavigationHelper

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			NavigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e) {
			NavigationHelper.OnNavigatedFrom(e);
		}

		#endregion
	}
}