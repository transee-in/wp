using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Transee {
    class ErrorPageArgs {
        public ErrorPageArgs(string textKey) {
            this.TextKey = textKey;
        }

        public string TextKey { get; set; }
        public Func<bool> CheckingFunction { get; set; }
        public Action SuccessFunction { get; set; }
    }

    public sealed partial class ErrorPage : Page {
        private ErrorPageArgs args;
        private ResourceLoader resourceLoader = new ResourceLoader();

        public ErrorPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            this.args = e.Parameter as ErrorPageArgs;
            this.ShowText(this.GetText());
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (args.CheckingFunction()) {
                args.SuccessFunction();
            } else {
                this.ShowText(TryHarder());
            }
        }

        private void ShowText(string text) {
            errorTextBlock.Text = text;
        }

        private string GetText() {
            return resourceLoader.GetString(this.args.TextKey);
        }

        private string TryHarder() {
            return resourceLoader.GetString("try_harder");
        }
    }
}
