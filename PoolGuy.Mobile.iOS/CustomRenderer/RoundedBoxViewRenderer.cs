using System;
using CoreGraphics;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedBoxView), typeof(RoundedBoxViewRenderer))]
namespace PoolGuy.Mobile.iOS.CustomRenderer
{
    public class RoundedBoxViewRenderer : BoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                this.SetNeedsDisplay();
            }
        }

        public override void Draw(CGRect rect)
        {
            RoundedBoxView rbv = (RoundedBoxView)this.Element;

            using (var context = UIGraphics.GetCurrentContext())
            {
                context.SetFillColor(rbv.BackgroundColor.ToCGColor());
                context.SetStrokeColor(rbv.BorderColor.ToCGColor());
                context.SetLineWidth((float)rbv.BorderThickness);

                var rc = this.Bounds.Inset((int)rbv.BorderThickness, (int)rbv.BorderThickness);

                float radius = (float)rbv.CornerRadius;
                radius = (float)Math.Max(0, Math.Min(radius, Math.Max(rc.Height / 2, rc.Width / 2)));
                try
                {
                    var path = CGPath.FromRoundedRect(rc, radius, radius);
                    context.AddPath(path);
                    context.DrawPath(CGPathDrawingMode.FillStroke);
                }
                catch (Exception) { }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            this.SetNeedsDisplay();
        }

        #region Old Code

        #endregion
    }
}