using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackgroundDrawable(gd);
                this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            }
        }
    }
}