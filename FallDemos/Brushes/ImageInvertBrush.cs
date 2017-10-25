using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System;
using Windows.Graphics.Effects;

namespace FallDemos.Brushes {
    public sealed class ImageInvertBrush : XamlCompositionBrushBase {

        private LoadedImageSurface _surface;
        private CompositionSurfaceBrush _surfaceBrush;

        public static readonly DependencyProperty ImageUriStringProperty = DependencyProperty.Register(nameof(ImageUriString), typeof(string), typeof(ImageInvertBrush), new PropertyMetadata(String.Empty, new PropertyChangedCallback(OnImageUriStringChanged)));
        public string ImageUriString {
            get => (String)GetValue(ImageUriStringProperty);
            set => SetValue(ImageUriStringProperty, value);
        }

        private static void OnImageUriStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            // Unbox and update surface if CompositionBrush exists     
            if (d is ImageInvertBrush brush && brush._surfaceBrush != null) {
                var newSurface = LoadedImageSurface.StartLoadFromUri(new Uri((String)e.NewValue));
                brush._surface = newSurface;
                brush._surfaceBrush.Surface = newSurface;
            }
        }

        protected override void OnConnected() {
            // return if Uri String is null or empty
            if (String.IsNullOrEmpty(ImageUriString)) return;

            Compositor compositor = Window.Current.Compositor;
            // Use LoadedImageSurface API to get ICompositionSurface from image uri provided
            _surface = LoadedImageSurface.StartLoadFromUri(new Uri(ImageUriString));
            // Load Surface onto SurfaceBrush
            _surfaceBrush = compositor.CreateSurfaceBrush(_surface);
            _surfaceBrush.Stretch = CompositionStretch.Uniform;

            // CompositionCapabilities: Are Effects supported?
            bool usingFallback = !CompositionCapabilities.GetForCurrentView().AreEffectsSupported();
            if (usingFallback) {
                // If Effects are not supported, Fallback to image without effects
                CompositionBrush = _surfaceBrush;
                return;
            }

            // 1 - Get the BackdropBrush
            // NOTE: BackdropBrush is what is behind the current UI element (also useful for Blur effects)
            var backdrop = Window.Current.Compositor.CreateBackdropBrush();

            // 2 - Create your Effect
            // New-up a Win2D InvertEffect and use the SurfaceBrush as it's Source
            var invertEffect = new InvertEffect {
                Source = new CompositionEffectSourceParameter("SurfaceBrush")
            };

            // 3 - Get an EffectFactory
            var effectFactory = Window.Current.Compositor.CreateEffectFactory(invertEffect);

            // 4 - Get a CompositionEffectBrush
            var effectBrush = effectFactory.CreateBrush();

            // and set the SurfaceBrush as the original source
            effectBrush.SetSourceParameter("SurfaceBrush", _surfaceBrush);

            // 5 - Finally, assign your CompositionEffectBrush to the XCBB's CompositionBrush property
            CompositionBrush = effectBrush;
        }

        protected override void OnDisconnected() {
            // Dispose Surface and CompositionBrushes if XamlCompBrushBase is removed from tree
            if (_surface != null) {
                _surface.Dispose();
                _surface = null;
            }
            if (CompositionBrush != null) {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }
    }

    public class InvertBrush : XamlCompositionBrushBase {
        protected override void OnConnected() {
            // Set up CompositionBrush
            base.OnConnected();
        }

        protected override void OnDisconnected() {
            // Clean up
            base.OnDisconnected();
        }
    }

}