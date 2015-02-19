using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Transee {
    class ErrorPageArgs {
        public ErrorPageArgs(string text) {
            this.Text = text;
        }

        public string Text { get; set; }
    }

    public sealed partial class ErrorPage : Page {
        public ErrorPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var args = e.Parameter as ErrorPageArgs;
            errorTextBlock.Text = args.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            App.ShowCityListOrCityPage();
        }
    }
}
