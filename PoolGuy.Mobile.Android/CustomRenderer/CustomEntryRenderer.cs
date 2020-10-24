using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Droid.CustomRenderer;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context)
            :base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            if (e.OldElement != null)
            {
                var entry = (CustomEntry)e.OldElement;

                Control.AfterTextChanged -= EnhancedEntryRenderer_AfterTextChanged;
                Control.EditorAction -= Control_EditorAction;
                entry.SelectAllTextEvent -= Entry_SelectAllTextEvent;
            }

            if (e.NewElement != null)
            {
                var entry = (CustomEntry)e.NewElement;
                entry.SelectAllTextEvent += Entry_SelectAllTextEvent;

                switch (entry.HorizontalTextAlignment)
                {
                    case Xamarin.Forms.TextAlignment.Center:
                        Control.Gravity = GravityFlags.CenterHorizontal;
                        break;
                    case Xamarin.Forms.TextAlignment.End:
                        Control.Gravity = GravityFlags.End;
                        break;
                    case Xamarin.Forms.TextAlignment.Start:
                        Control.Gravity = GravityFlags.Start;
                        break;
                }

                switch (entry.Capitalization)
                {
                    case CapitalizationType.None:
                        break;
                    case CapitalizationType.Characters:
                        Control.InputType = Android.Text.InputTypes.TextFlagCapCharacters;
                        break;
                    case CapitalizationType.Sentences:
                        Control.InputType = Android.Text.InputTypes.TextFlagCapSentences;
                        break;
                    case CapitalizationType.Words:
                        Control.InputType = Android.Text.InputTypes.TextFlagCapWords;
                        break;
                    default:
                        break;
                }

                //Allowing just numeric values if Numeric Keyboard
                if (entry.Keyboard == Xamarin.Forms.Keyboard.Numeric)
                {
                    this.Control.KeyListener = DigitsKeyListener.GetInstance("1234567890.");
                }

                if (entry.SelectAll)
                {
                    Control.SetSelectAllOnFocus(true);
                }
                else if (!entry.SelectAll && (entry.Keyboard == Xamarin.Forms.Keyboard.Numeric || entry.Keyboard == Xamarin.Forms.Keyboard.Telephone))
                {
                    Control.AfterTextChanged += EnhancedEntryRenderer_AfterTextChanged;
                }

                SetBottomLine(entry);
                SetPadding(entry);
                SetReturnType(entry);
                Control.EditorAction += Control_EditorAction;
                Control.TextSize = (float)entry.FontSize;
                SetNativeControl(Control);
            }
        }

        private void Entry_SelectAllTextEvent(object sender, EventArgs e)
        {
            //((EntryEditText)Control).SelectAll();
            ((Xamarin.Forms.Platform.Android.FormsEditText)Control).SelectAll();
        }

        private void Control_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            var entry = (CustomEntry)Element;
            switch (entry.ReturnType)
            {
                case CustomControls.ReturnType.Default:
                    break;
                case CustomControls.ReturnType.Go:
                    entry.Unfocus();
                    entry.InvokeGoTo();
                    break;
                case CustomControls.ReturnType.Next:
                    entry.Unfocus();
                    entry.InvokeNext();
                    break;
                case CustomControls.ReturnType.Previous:
                    entry.Unfocus();
                    entry.InvokePrevious();
                    break;
                case CustomControls.ReturnType.Done:
                    entry.Unfocus();
                    break;
                case CustomControls.ReturnType.Send:
                    entry.Unfocus();
                    entry.InvokeGoTo();
                    break;
                case CustomControls.ReturnType.Search:
                    entry.Unfocus();
                    entry.InvokeGoTo();
                    break;
                default:
                    break;
            }
        }

        private void EnhancedEntryRenderer_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            ((global::Android.Widget.EditText)sender).SetSelection(((global::Android.Widget.EditText)sender).Text.Length);
        }

        private void SetBottomLine(CustomEntry view)
        {
            if (!view.HasBorder)
            {
                this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }

        private void SetPadding(CustomEntry view)
        {
            if (!view.RemovePadding)
            {
                return;
            }

            this.Control.SetPadding(0, 0, 0, 0);
        }

        private void SetReturnType(CustomEntry entry)
        {
            CustomControls.ReturnType type = entry.ReturnType;

            switch (type)
            {
                case CustomControls.ReturnType.Go:
                    Control.ImeOptions = ImeAction.Go;
                    Control.SetImeActionLabel("Go", ImeAction.Go);
                    break;
                case CustomControls.ReturnType.Next:
                    Control.ImeOptions = ImeAction.Next;
                    Control.SetImeActionLabel("Next", ImeAction.Next);
                    break;
                case CustomControls.ReturnType.Previous:
                    Control.ImeOptions = ImeAction.Previous;
                    Control.SetImeActionLabel("Prev", ImeAction.Previous);
                    break;
                case CustomControls.ReturnType.Send:
                    Control.ImeOptions = ImeAction.Send;
                    Control.SetImeActionLabel("Send", ImeAction.Send);
                    break;
                case CustomControls.ReturnType.Search:
                    Control.ImeOptions = ImeAction.Search;
                    Control.SetImeActionLabel("Search", ImeAction.Search);
                    break;
                case CustomControls.ReturnType.Done:
                    Control.ImeOptions = ImeAction.Done;
                    Control.SetImeActionLabel("Done", ImeAction.Done);
                    break;
                default:
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.AfterTextChanged -= EnhancedEntryRenderer_AfterTextChanged;
                Control.EditorAction -= Control_EditorAction;
                ((CustomEntry)Element).SelectAllTextEvent -= Entry_SelectAllTextEvent;
            }
            base.Dispose(disposing);
        }
    }
}