using Transee.Common;
using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Transee.API;

namespace Transee {
    public sealed partial class App : Application {
        private TransitionCollection _transitions;

        public App() {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e) {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached) {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null) {
                rootFrame = new Frame();

                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                rootFrame.CacheSize = 1;
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    try {
                        await SuspensionManager.RestoreAsync();
                    } catch (SuspensionManagerException) {
                        // Возникли ошибки при восстановлении состояния.
                        // Предполагаем, что состояние отсутствует, и продолжаем.
                    }
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null) {
                if (rootFrame.ContentTransitions != null) {
                    _transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions) {
                        _transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;

                ShowCityListOrCityPage();
            }

            Window.Current.Activate();
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e) {
            var rootFrame = sender as Frame;
	        if (rootFrame == null) {
		        return;
	        }
	        rootFrame.ContentTransitions = _transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
	        rootFrame.Navigated -= RootFrame_FirstNavigated;
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        public static async void ShowCityListOrCityPage() {
            var settings = ApplicationData.Current.LocalSettings;
            var rootFrame = Window.Current.Content as Frame;
			if (rootFrame == null) {
				return;
			}

			if (IsInternetAvailable) {
                var defaultCityId = settings.Values["CityId"];

                if (defaultCityId != null) {
                    if (!rootFrame.Navigate(typeof(CityPage), defaultCityId)) {
                        throw new Exception("Failed to create initial page");
                    }
                } else {
                    var cities = await CitiesFetcher.GetAsync();

                    if (!rootFrame.Navigate(typeof(CityListPage), cities)) {
                        throw new Exception("Failed to create initial page");
                    }
                }
            } else {
                var args = new ErrorPageArgs("internet_connection_error") {
                    CheckingFunction = () => IsInternetAvailable,
                    SuccessFunction = ShowCityListOrCityPage
                };

                if (!rootFrame.Navigate(typeof(ErrorPage), args)) {
                    throw new Exception("Failed to create initial page");
                }
            }
        }

        public static bool IsInternetAvailable {
            get {
                var profiles = NetworkInformation.GetConnectionProfiles();
                var internetProfile = NetworkInformation.GetInternetConnectionProfile();
                return profiles.Any(s => s.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                    || (internetProfile != null
                            && internetProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
            }
        }
    }
}
