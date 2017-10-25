using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FallDemos {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
            this.Loaded += (s, e) => {
                menu.SelectedItem = menu.MenuItems[0];
            };
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetPreferredMinSize(new Windows.Foundation.Size { Width = 500, Height = 500 });

            //PC customization
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView")) {
                var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null) {
                    var color = (Windows.UI.Color)App.Current.Resources["SystemAccentColor"];
                    titleBar.ButtonBackgroundColor = color;
                    titleBar.BackgroundColor = color;

                    titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
                    titleBar.ForegroundColor = Windows.UI.Colors.White;

                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Colors.DarkGray;
                    titleBar.ButtonPressedBackgroundColor = Windows.UI.Colors.Gray;
                }
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) => {
                if (this.ContentFrame.CanGoBack) {
                    e.Handled = true;
                    this.ContentFrame.GoBack();
                }
            };
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e) {
            if (this.ContentFrame.CanGoBack) {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            } else {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
            var nvi = args.SelectedItem as NavigationViewItem;
            var tag = nvi.Tag as string ?? "SettingsPage";
            try {
                var type = Type.GetType($"FallDemos.Views.Pages.{tag}");
                this.ContentFrame.Navigate(type);
                sender.Header = nvi.Content;
            } catch { }
        }
    }
}
