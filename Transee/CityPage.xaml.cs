using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Transee.API;
using Transee.Common;
using Transee.DataModel.CityInfo;
using Type = Transee.DataModel.CityInfo.Type;

namespace Transee {
	public sealed partial class CityPage {
		public NavigationHelper NavigationHelper { get; }
		public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();

        private readonly ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;
        private readonly ResourceLoader _resourceLoader = new ResourceLoader();
		private readonly Status _status = new Status();
		private readonly Dictionary<string, Type> _selectedTransports = new Dictionary<string, Type>();
		private string _cityId;

		public CityPage() {
			InitializeComponent();

			NavigationHelper = new NavigationHelper(this);
			NavigationHelper.LoadState += NavigationHelper_LoadState;
			NavigationHelper.SaveState += NavigationHelper_SaveState;
		}

		private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) {
			_cityId = e.NavigationParameter.ToString();
			var cityName = _resourceLoader.GetString(_cityId);
			var transports = await CityInfoFetcher.GetAsync(_cityId);

			// TODO: add page with favorite transports
			DefaultViewModel["CityName"] = cityName;
			DefaultViewModel["Transports"] = transports.Items;
		}

		private static void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
		}

		private void AppBarButton_ClickCities(object sender, RoutedEventArgs e) {
		    _settings.Values["CityID"] = null;

            if (!Frame.Navigate(typeof (CityListPage))) {
				var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
				throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
			}
		}

		private void AppBarButton_ClickNext(object sender, RoutedEventArgs e) {
			var types = new Dictionary<string, List<string>>();

			_status.ShowStatusBar("load_positions");

			foreach (var type in _selectedTransports) {
				var transportIds = type.Value.Items.Select(element => element.Id).ToList();
				types.Add(type.Key, transportIds);
			}

			// var positions = await PositionsFetcher.GetAsync(this.CityId, types);
			var positionTypes = new MapPageArgs(_cityId, types);

			_status.HideStatusBar();

			if (!Frame.Navigate(typeof (MapPage), positionTypes)) {
				var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
				throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
			}
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var listView = sender as ListView;
			if (listView == null) {
				return;
			}

			var cityInfoType = listView.DataContext as Type;
			var transportTypeId = cityInfoType?.Id;

			var transportType = new Type {
				Id = transportTypeId,
				Items = new List<TypeItem>()
			};

			foreach (TypeItem item in listView.SelectedItems) {
				transportType.Items.Add(item);
			}

			if (_selectedTransports.ContainsKey(transportTypeId)) {
				_selectedTransports.Remove(transportTypeId);
			}

			_selectedTransports.Add(transportTypeId, transportType);
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