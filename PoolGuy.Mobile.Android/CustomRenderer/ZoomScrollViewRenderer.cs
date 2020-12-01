using Android.Views;
using PoolGuy.Mobile.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.ScaleGestureDetector;
using Android.Views.Animations;
using Android.Content;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ScrollView), typeof(ZoomScrollViewRenderer))]
namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    public class ZoomScrollViewRenderer : ScrollViewRenderer, IOnScaleGestureListener
    {
        private float mScale = 1f;
        private ScaleGestureDetector mScaleDetector;

        public ZoomScrollViewRenderer(Context context)
            :base(context)
        {

        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {

            base.OnElementChanged(e);
            mScaleDetector = new ScaleGestureDetector(Context, this);

        }


        public override bool DispatchTouchEvent(MotionEvent e)
        {
            base.DispatchTouchEvent(e);
            return mScaleDetector.OnTouchEvent(e);
        }

        public bool OnScale(ScaleGestureDetector detector)
        {
            float scale = 1 - detector.ScaleFactor;

            float prevScale = mScale;
            mScale += scale;

            if (mScale < 0.5f) // Minimum scale condition:
                mScale = 0.5f;

            if (mScale > 1f) // Maximum scale condition:
                mScale = 1f;
            ScaleAnimation scaleAnimation = new ScaleAnimation(1f / prevScale, 1f / mScale, 1f / prevScale, 1f / mScale, detector.FocusX, detector.FocusY);
            scaleAnimation.Duration = 0;
            scaleAnimation.FillAfter = true;
            StartAnimation(scaleAnimation);
            return true;
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {

        }
    }
}