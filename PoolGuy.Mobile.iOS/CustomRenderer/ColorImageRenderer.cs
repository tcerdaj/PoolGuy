using CoreGraphics;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.iOS.CustomRenderer;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRendererAttribute(typeof(ColorImage), typeof(ColorImageRenderer))]
namespace PoolGuy.Mobile.iOS.CustomRenderer
{
    public class ColorImageRenderer : ViewRenderer<ColorImage, UIImageView>
    {
        private bool _isDisposed;

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing && base.Control != null)
            {
                UIImage image = base.Control.Image;
                UIImage uIImage = image;
                if (image != null)
                {
                    uIImage.Dispose();
                    uIImage = null;
                }
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ColorImage> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                UIImageView uIImageView = new UIImageView(CGRect.Empty)
                {
                    ContentMode = UIViewContentMode.ScaleAspectFit,
                    ClipsToBounds = true
                };
                SetNativeControl(uIImageView);
            }
            if (e.NewElement != null)
            {
                SetImage(e.OldElement);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ColorImage.SourceProperty.PropertyName)
            {
                SetImage(null);
            }
            else if (e.PropertyName == ColorImage.ForegroundProperty.PropertyName)
            {
                SetImage(null);
            }
        }

        private void SetImage(ColorImage previous = null)
        {
            if (previous == null)
            {
                var uiImage = new UIImage(Element.Source);
                uiImage = uiImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Control.TintColor = Element.Foreground.ToUIColor();
                Control.Image = uiImage;
                if (!_isDisposed)
                {
                    ((IVisualElementController)Element).NativeSizeChanged();
                }
            }
        }
    }
}