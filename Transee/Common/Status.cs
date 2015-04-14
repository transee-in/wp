using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.ViewManagement;

namespace Transee.Common {
    class Status {
	    readonly StatusBar _statusBar = StatusBar.GetForCurrentView();
	    readonly ResourceLoader _resourceLoader = new ResourceLoader();

        public async void ShowStatusBar(string key) {
            _statusBar.ProgressIndicator.Text = _resourceLoader.GetString(key);
            await _statusBar.ProgressIndicator.ShowAsync();
        }

        public async void HideStatusBar() {
            await _statusBar.ProgressIndicator.HideAsync();
        }
    }
}
