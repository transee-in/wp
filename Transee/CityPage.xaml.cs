using Transee.Common;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using Transee.DataModel.CityInfo;
using System;
using System.Diagnostics;
using System.Linq;
using Transee.API;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Transee {
    public sealed partial class CityPage : Page {
        private readonly NavigationHelper _navigationHelper;
        private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader _resourceLoader = new ResourceLoader();
        private readonly Status _status = new Status();
        private readonly Dictionary<string, DataModel.CityInfo.Type> _selectedTransports = new Dictionary<string, DataModel.CityInfo.Type>();
        private string _cityId;

        public CityPage() {
	        InitializeComponent();

			_navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
            _navigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper => _navigationHelper;

	    public ObservableDictionary DefaultViewModel => _defaultViewModel;

	    private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) {
            _cityId = e.NavigationParameter.ToString();
            var cityName = _resourceLoader.GetString(_cityId);
            var transports = await CityInfoFetcher.GetAsync(_cityId);

            // TODO: add page with favorite transports
            DefaultViewModel["CityName"] = cityName;
            DefaultViewModel["Transports"] = transports.Items;
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

            _status.ShowStatusBar("load_positions");

            foreach (var type in _selectedTransports) {
                var transportIds = type.Value.Items.Select(element => element.Id).ToList();
	            types.Add(type.Key, transportIds);
            }

            // var positions = await PositionsFetcher.GetAsync(this.CityId, types);
            var positionTypes = new MapPageArgs(_cityId, types);

            _status.HideStatusBar();

            if (!Frame.Navigate(typeof(MapPage), positionTypes)) {
                var resourceLoader = ResourceLoader.GetForCurrentView("Resources");
                throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var listView = sender as ListView;
			if (listView == null) {
				return;
			}

			var cityInfoType = listView.DataContext as DataModel.CityInfo.Type;
			var transportTypeId = cityInfoType?.Id;

			var transportType = new DataModel.CityInfo.Type {
				Id = transportTypeId, Items = new List<TypeItem>()
			};

			foreach (TypeItem item in listView.SelectedItems) {
				transportType.Items.Add(item);
			}

			if (_selectedTransports.ContainsKey(transportTypeId)) {
				_selectedTransports.Remove(transportTypeId);
			}

			_selectedTransports.Add(transportTypeId, transportType);
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var listBox = sender as ListBox;
	        if (listBox == null) {
		        return;
	        }

			Debug.WriteLine(listBox);

			if (listBox.IsEnabled) {
				listBox.Background = new SolidColorBrush(Colors.Aquamarine);
			} else {
				listBox.Background = new SolidColorBrush(Colors.Transparent);
			}

	        var cityInfoType = listBox.DataContext as DataModel.CityInfo.Type;
	        var transportTypeId = cityInfoType?.Id;

	        var transportType = new DataModel.CityInfo.Type {
		        Id = transportTypeId, Items = new List<TypeItem>()
	        };

	        foreach (TypeItem item in listBox.SelectedItems) {
		        transportType.Items.Add(item);
	        }

	        if (_selectedTransports.ContainsKey(transportTypeId)) {
		        _selectedTransports.Remove(transportTypeId);
	        }

	        _selectedTransports.Add(transportTypeId, transportType);
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
