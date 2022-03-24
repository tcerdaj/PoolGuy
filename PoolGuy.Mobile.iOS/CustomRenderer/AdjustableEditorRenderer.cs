using System.Drawing;
using FieldEdge.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PoolGuy.Mobile.CustomControls;

[assembly: ExportRenderer(typeof(AdjustableEditor), typeof(AdjustableEditorRenderer))]
namespace FieldEdge.Mobile.iOS.CustomRenderers
{
    public class AdjustableEditorRenderer : EditorRenderer //ViewRenderer<AdjustableEditor, UITextView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null || Control == null)
            {
                return;
            }

            var view = e.NewElement as AdjustableEditor;
            Control.ScrollEnabled = view.ScrollEnabled;

            #region HasCancel & HasDone

            if (view.HasCancel && view.HasDone)
            {
                UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)Frame.Size.Width, 44.0f)) { Translucent = true };
                toolbar.Items = new[]
                {
                    new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, delegate {Control.ResignFirstResponder(); }),
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem("Done",UIBarButtonItemStyle.Done, delegate {view.InvokeDone(); Control.ResignFirstResponder(); })
                };
                Control.InputAccessoryView = toolbar;
            }

            #endregion
        }
    }
}