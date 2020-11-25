using SkiaSharp.Views.Forms;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using SkiaSharp;
using System.Diagnostics;

namespace PoolGuy.Mobile.CustomControls
{
    public class Spinner: SKCanvasView
    {
        private SKCanvasView _canvasView;
        private int _colorIndex;
        private static Color _originalBackgroundColor;
        private static bool _isActive;

        public ObservableCollection<Color> Colors { get; } = new ObservableCollection<Color>();

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly BindableProperty RadiusProperty =
    BindableProperty.Create(nameof(Radius), typeof(int), typeof(Spinner), 0);

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly BindableProperty IsActiveProperty =
    BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(Spinner), false, BindingMode.OneWayToSource, propertyChanged: OnIsActiveChanged);

        public Spinner()
        {
            _canvasView = this;
            _colorIndex = 0;
            _isActive = false;
            Colors.Add((Color)Application.Current.Resources["Title"]);
            Colors.Add((Color)Application.Current.Resources["Primary"]);
            Debug.WriteLine($"Spinner...Original BackgroundColor:{BackgroundColor.ToHex()}");
            _originalBackgroundColor = this.BackgroundColor;
            Colors.CollectionChanged += Colors_CollectionChanged;
        }

        private static void OnIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var scanner = bindable as Spinner;

            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                if (oldValue != newValue && scanner.IsActive)
                    scanner.InvalidateSurface();

                Debug.WriteLine($"scanner.IsActive:{scanner.IsActive}");
                _isActive = scanner.IsActive;
                if (!_isActive)
                {
                    scanner.IsVisible = false;
                }
                else if(!scanner.IsVisible) 
                {
                    scanner.IsVisible = true;
                }
               
                return scanner.IsActive;
            });
        }

        private void Colors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InitializeColors();
        }

        private void InitializeColors()
        {
            this.BackgroundColor = Colors[Colors.Count() - 1];
            _canvasView.InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            OnPainting(this, e);
        }

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);

            SKPaint paint = new SKPaint
            {
                IsAntialias = false,
                Style = SKPaintStyle.Fill,
                Color = Colors[_colorIndex].ToSKColor()
            };

            canvas.DrawCircle(center.X, center.Y, Radius, paint);

            Radius += 40;

            int hRectInCircle = (int)((Radius / Math.Sqrt(2)) * 2);
            
            Debug.WriteLine($"OnPainting...IsActive:{_isActive}");
            if (hRectInCircle >= Math.Max(info.Width, info.Height))
            {
                this.BackgroundColor = Colors[_colorIndex];
                Debug.WriteLine($"OnPainting...BackgroundColor:{this.BackgroundColor.ToHex()}");

                if (_colorIndex >= Colors.Count() - 1)
                    _colorIndex = 0;
                else
                    _colorIndex++;

                Radius = 0;
            }
        }
    }
}