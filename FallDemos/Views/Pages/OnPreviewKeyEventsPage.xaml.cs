using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FallDemos.Views.Pages {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnPreviewKeyEventsPage : Page {
        public OnPreviewKeyEventsPage() {
            this.InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e) {
            e.Handled = IsNotNumberic(e.Key);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e) {
            e.Handled = IsNotNumberic(e.Key) &&
                !(e.Key == VirtualKey.Back || e.Key == VirtualKey.Tab ||
                    e.Key == VirtualKey.Delete || e.Key == VirtualKey.Left ||
                    e.Key == VirtualKey.Right);
        }

        private static bool IsNotNumberic(VirtualKey key) {
            return (key < VirtualKey.Number0 || key > VirtualKey.Number9) &&
                    (key < VirtualKey.NumberPad0 || key > VirtualKey.NumberPad9);
        }

        private void TextBox_Paste(object sender, TextControlPasteEventArgs e) {
            e.Handled = true;
        }
    }
}
