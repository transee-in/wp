using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Transee {
	internal class ErrorPageArgs {
		public ErrorPageArgs(string textKey) {
			TextKey = textKey;
		}

		public string TextKey { get; set; }
		public Func<bool> CheckingFunction { get; set; }
		public Action SuccessFunction { get; set; }
	}

	public sealed partial class ErrorPage {
		private ErrorPageArgs _args;
		private readonly ResourceLoader _resourceLoader = new ResourceLoader();

		public ErrorPage() {
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			_args = e.Parameter as ErrorPageArgs;
			ShowText(GetText());
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			if (_args.CheckingFunction()) {
				_args.SuccessFunction();
			} else {
				ShowText(TryHarder());
			}
		}

		private void ShowText(string text) {
			errorTextBlock.Text = text;
		}

		private string GetText() {
			return _resourceLoader.GetString(_args.TextKey);
		}

		private string TryHarder() {
			return _resourceLoader.GetString("try_harder");
		}
	}
}