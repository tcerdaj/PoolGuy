using Android.Graphics;
using Android.Graphics.Drawables;
using System;

namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    public class NativeDragShadowBuilder : Android.Views.View.DragShadowBuilder
    {
        private readonly Drawable shadow;

        public NativeDragShadowBuilder(Android.Views.View v)
            : base(v)
        {
            v.DrawingCacheEnabled = true;
            Bitmap bm = v.DrawingCache;
            //shadow = new BitmapDrawable(bm);
            //shadow.SetColorFilter(Color.ParseColor("#4EB1FB"), PorterDuff.Mode.Multiply);
            shadow = GetAndAddHoverView(v);
        }

        public override void OnProvideShadowMetrics(Point size, Point touch)
        {
            int width = View.Width;
            int height = View.Height;
            shadow.SetBounds(0, 0, width, height);
            size.Set(width, height);
            touch.Set(width / 2, height / 2);
        }

        public override void OnDrawShadow(Canvas canvas)
        {
            base.OnDrawShadow(canvas);
            shadow.Draw(canvas);
        }

        #region Bitmap Drawable Creation

        /// <summary>
        /// Creates the hover cell with the appropriate bitmap and of appropriate
        /// size. The hover cell's BitmapDrawable is drawn on top of the bitmap every
        /// single time an invalidate call is made.
        /// </summary>
        private BitmapDrawable GetAndAddHoverView(Android.Views.View v)
        {

            int w = v.Width;
            int h = v.Height;
            int top = v.Top;
            int left = v.Left;

            Bitmap b = GetBitmapWithBorder(v);

            BitmapDrawable drawable = new BitmapDrawable(b);

            drawable.SetBounds(left, top, left + w, top + h);

            //Set the moveable view to transparent
            drawable.SetAlpha(180);
            return drawable;
        }

        /// <summary>
        /// Draws a red border over the screenshot of the view passed in.
        /// </summary>
        private static Bitmap GetBitmapWithBorder(Android.Views.View v)
        {
            Bitmap bitmap = GetBitmapFromView(v);
            Canvas can = new Canvas(bitmap);

            Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height);

            Paint paint = new Paint();
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = 10;
            paint.Color = Android.Graphics.Color.Rgb(29, 29, 29);

            can.DrawBitmap(bitmap, 0, 0, null);
            can.DrawRect(rect, paint);

            return bitmap;
        }

        /// <summary>
        /// Returns a bitmap showing a screenshot of the view passed in
        /// </summary>
        private static Bitmap GetBitmapFromView(Android.Views.View v)
        {
            try
            {
                Bitmap bitmap = Bitmap.CreateBitmap(v.Width, v.Height, Bitmap.Config.Argb8888);
                Canvas canvas = new Canvas(bitmap);

                v.Draw(canvas);

                return bitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default(Bitmap);

        }

        #endregion
    }
}