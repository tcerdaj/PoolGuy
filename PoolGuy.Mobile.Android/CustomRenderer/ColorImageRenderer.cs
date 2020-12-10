using Android.Content;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System.ComponentModel;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Droid.CustomRenderer;

[assembly: ExportRendererAttribute(typeof(ColorImage), typeof(ColorImageRenderer))]
namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    public class ColorImageRenderer : ViewRenderer<ColorImage, ImageView>
    {
        private bool _isDisposed;

        public ColorImageRenderer(Context context) : base(context)
        {
            base.AutoPackage = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ColorImage> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                SetNativeControl(new ImageView(Context));
            }
            UpdateBitmap(e.OldElement);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ColorImage.SourceProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
            else if (e.PropertyName == ColorImage.ForegroundProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
        }

        private void UpdateBitmap(ColorImage previous = null)
        {
            try
            {
                if (!_isDisposed)
                {
                    var d = ResourceManager.GetDrawable(Android.App.Application.Context, Element.Source).Mutate();
                    if (d != null)
                    {
                        d.SetColorFilter(new LightingColorFilter(Element.Foreground.ToAndroid(), Element.Foreground.ToAndroid()));
                        d.Alpha = Element.Foreground.ToAndroid().A;
                        Control.SetImageDrawable(d);
                    }
                        ((IVisualElementController)Element).NativeSizeChanged();
                }
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}