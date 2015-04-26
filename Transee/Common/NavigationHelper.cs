using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Transee.Common {
	[Windows.Foundation.Metadata.WebHostHidden]
	public class NavigationHelper : DependencyObject {
		private Page Page { get; }
		private Frame Frame => Page.Frame;

		public NavigationHelper(Page page) {
			Page = page;

			Page.Loaded += (sender, e) => Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
			Page.Unloaded += (sender, e) => Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
		}

		private RelayCommand _goBackCommand;
		private RelayCommand _goForwardCommand;

		public RelayCommand GoBackCommand {
			get { return _goBackCommand ?? (_goBackCommand = new RelayCommand(GoBack, CanGoBack)); }
			set { _goBackCommand = value; }
		}

		public RelayCommand GoForwardCommand
			=> _goForwardCommand ?? (_goForwardCommand = new RelayCommand(GoForward, CanGoForward));


		public virtual bool CanGoBack() {
			return Frame != null && Frame.CanGoBack;
		}

		public virtual bool CanGoForward() {
			return Frame != null && Frame.CanGoForward;
		}

		public virtual void GoBack() {
			if (Frame != null && Frame.CanGoBack) {
				Frame.GoBack();
			}
		}

		public virtual void GoForward() {
			if (Frame != null && Frame.CanGoForward) {
				Frame.GoForward();
			}
		}

		private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e) {
			if (!GoBackCommand.CanExecute(null)) {
				return;
			}
			e.Handled = true;
			GoBackCommand.Execute(null);
		}

		private string _pageKey;

		public event LoadStateEventHandler LoadState;
		public event SaveStateEventHandler SaveState;

		public void OnNavigatedTo(NavigationEventArgs e) {
			var frameState = SuspensionManager.SessionStateForFrame(Frame);
			_pageKey = "Page-" + Frame.BackStackDepth;

			if (e.NavigationMode == NavigationMode.New) {
				var nextPageKey = _pageKey;
				var nextPageIndex = Frame.BackStackDepth;
				while (frameState.Remove(nextPageKey)) {
					nextPageIndex++;
					nextPageKey = "Page-" + nextPageIndex;
				}

				LoadState?.Invoke(this, new LoadStateEventArgs(e.Parameter, null));
			} else {
				LoadState?.Invoke(this, new LoadStateEventArgs(e.Parameter, (Dictionary<string, object>) frameState[_pageKey]));
			}
		}

		public void OnNavigatedFrom(NavigationEventArgs e) {
			var frameState = SuspensionManager.SessionStateForFrame(Frame);
			var pageState = new Dictionary<string, object>();
			SaveState?.Invoke(this, new SaveStateEventArgs(pageState));
			frameState[_pageKey] = pageState;
		}
	}

	public delegate void LoadStateEventHandler(object sender, LoadStateEventArgs e);

	public delegate void SaveStateEventHandler(object sender, SaveStateEventArgs e);

	public class LoadStateEventArgs : EventArgs {
		public object NavigationParameter { get; private set; }
		public Dictionary<string, object> PageState { get; private set; }

		public LoadStateEventArgs(object navigationParameter, Dictionary<string, object> pageState) {
			NavigationParameter = navigationParameter;
			PageState = pageState;
		}
	}

	public class SaveStateEventArgs : EventArgs {
		public Dictionary<string, object> PageState { get; private set; }

		public SaveStateEventArgs(Dictionary<string, object> pageState) {
			PageState = pageState;
		}
	}
}