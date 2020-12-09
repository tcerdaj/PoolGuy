using Android.Content;
using Android.Graphics;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Droid.CustomRenderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Rect = Android.Graphics.Rect;

[assembly: ExportRenderer(typeof(RoundedBoxView), typeof(RoundedBoxViewRenderer))]
namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    public class RoundedBoxViewRenderer : BoxRenderer
    {
        public RoundedBoxViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            SetWillNotDraw(false);

            Invalidate();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Invalidate();
        }

        public override void Draw(Canvas canvas)
        {
            RoundedBoxView rbv = (RoundedBoxView)this.Element;

            Rect rc = new Rect();
            GetDrawingRect(rc);

            Rect interior = rc;
            interior.Inset((int)rbv.BorderThickness, (int)rbv.BorderThickness);

            Paint p = new Paint()
            {
                Color = rbv.BackgroundColor.ToAndroid(),
                AntiAlias = true,
            };

            var radius = (float)(rc.Width() / rbv.Width * rbv.CornerRadius);

            //canvas.DrawRoundRect(new RectF(interior), (float)rbv.CornerRadius, (float)rbv.CornerRadius, p);
            canvas.DrawRoundRect(new RectF(interior), radius, radius, p);

            // Draw border
            if (rbv.BorderThickness > 0)
            {
                p.Color = rbv.BorderColor.ToAndroid();
                p.StrokeWidth = (float)rbv.BorderThickness;
                p.SetStyle(Paint.Style.Stroke);

                //canvas.DrawRoundRect(new RectF(rc), (float)rbv.CornerRadius, (float)rbv.CornerRadius, p);
                canvas.DrawRoundRect(new RectF(rc), radius, radius, p);
            }
        }
    }
}

