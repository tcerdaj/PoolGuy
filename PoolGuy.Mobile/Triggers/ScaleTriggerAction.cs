using Xamarin.Forms;

namespace PoolGuy.Mobile.Triggers
{
    public class ScaleDownTriggerAction : TriggerAction<VisualElement>
    {
        protected override async void Invoke(VisualElement sender)
        {
            await sender?.ScaleTo(0.95, 50, Easing.CubicOut);
        }
    }

    public class ScaleUpTriggerAction : TriggerAction<VisualElement>
    {
        protected override async void Invoke(VisualElement sender)
        {
            await sender?.ScaleTo(1.0, 50, Easing.CubicIn);
        }
    }
}
