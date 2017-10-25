using System;
using System.Threading.Tasks;

using FallDemos.Helpers;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.ApplicationModel.Core;

namespace FallDemos.Services
{
    public static class ThemeSelectorService
    {
        private const string SettingsKey = "RequestedTheme";

        public static event EventHandler<ElementTheme> OnThemeChanged = (sender, args) => { };

        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        private static readonly SolidColorBrush _baseBrush = Application.Current.Resources["ThemeControlForegroundBaseHighBrush"] as SolidColorBrush;

        public static SolidColorBrush GetSystemControlForegroundForTheme()
        {
            return _baseBrush;
        }

        public static async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();
            SetRequestedTheme();
        }

        public static async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            SetRequestedTheme();
            await SaveThemeInSettingsAsync(Theme);

            OnThemeChanged(null, Theme);
        }

        public static void SetRequestedTheme()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = Theme;
            }
            SetupTitlebar();
        }

        private static void SetupTitlebar() {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView")) {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null) {
                    titleBar.ButtonBackgroundColor = Colors.Transparent;
                    if (TrueTheme() == ElementTheme.Dark) {
                        titleBar.ButtonForegroundColor = Colors.White;
                        titleBar.ForegroundColor = Colors.White;
                    } else {
                        titleBar.ButtonForegroundColor = Colors.Black;
                        titleBar.ForegroundColor = Colors.Black;
                    }

                    titleBar.BackgroundColor = Colors.Black;

                    titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                    titleBar.ButtonInactiveForegroundColor = Colors.LightGray;

                    CoreApplicationViewTitleBar coreTitleBar = TitleBarHelper.Instance.TitleBar;

                    coreTitleBar.ExtendViewIntoTitleBar = true;
                }
            }
        }

        public static ElementTheme TrueTheme() {
            var frameworkElement = Window.Current.Content as FrameworkElement;
            return frameworkElement.ActualTheme;
        }

        private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
        }
    }
}
