using Android.Content;
using Android.Views;
using Xamarin.Forms;
using FieldEdge.Mobile.Droid.CustomRenderers;
using Android.Text.Method;
using Xamarin.Forms.Platform.Android;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Droid.Listener;

[assembly: ExportRenderer(typeof(AdjustableEditor), typeof(AdjustableEditorRenderer))]
namespace FieldEdge.Mobile.Droid.CustomRenderers
{
    public class AdjustableEditorRenderer : EditorRenderer
    {
        public AdjustableEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                var view = e.NewElement as AdjustableEditor;
                if (!view.HasBorder)
                {
                    this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                }

                if (view.ScrollEnabled)
                {
                    var nativeEditText = (global::Android.Widget.EditText)Control;

                    //While scrolling inside Editor stop scrolling parent view.
                    nativeEditText.OverScrollMode = OverScrollMode.Always;
                    nativeEditText.ScrollBarStyle = ScrollbarStyles.InsideInset;
                    nativeEditText.SetOnTouchListener(new DroidTouchListener());

                    //For Scrolling in Editor innner area
                    Control.VerticalScrollBarEnabled = true;
                    Control.MovementMethod = ScrollingMovementMethod.Instance;
                    Control.ScrollBarStyle = Android.Views.ScrollbarStyles.InsideInset;
                    //Force scrollbars to be displayed
                    Android.Content.Res.TypedArray a = Control.Context.Theme.ObtainStyledAttributes(new int[0]);
                    InitializeScrollbars(a);
                    a.Recycle();
                }

                SetPadding(view);
                SetNativeControl(Control);
            }
        }

        private void SetPadding(AdjustableEditor view)
        {
            if (!view.RemovePadding)
            {
                return;
            }

            Control.SetPadding(0, 0, 0, 0);
        }
    }
}