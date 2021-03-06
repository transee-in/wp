﻿using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.ViewManagement;

namespace Transee.Common {
	internal class Status {
		private readonly StatusBar _statusBar = StatusBar.GetForCurrentView();
		private readonly ResourceLoader _resourceLoader = new ResourceLoader();

		public async void ShowStatusBar(string key) {
			_statusBar.ProgressIndicator.Text = _resourceLoader.GetString(key);
			await _statusBar.ProgressIndicator.ShowAsync();
		}

		public async void HideStatusBar() {
			await _statusBar.ProgressIndicator.HideAsync();
		}
	}
}