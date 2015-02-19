using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.ViewManagement;

namespace Transee.Common {
    class Status {
        StatusBar statusBar = StatusBar.GetForCurrentView();
        ResourceLoader resourceLoader = new ResourceLoader();

        public async void ShowStatusBar(string key) {
            statusBar.ProgressIndicator.Text = resourceLoader.GetString(key);
            await statusBar.ProgressIndicator.ShowAsync();
        }

        public async void HideStatusBar() {
            await statusBar.ProgressIndicator.HideAsync();
        }
    }
}
