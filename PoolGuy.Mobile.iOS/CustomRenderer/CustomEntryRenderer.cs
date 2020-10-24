using Foundation;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.iOS.CustomRenderer;
using System;
using System.ComponentModel;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ObjCRuntime;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace PoolGuy.Mobile.iOS.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        bool _nativeSelectionIsUpdating;
        bool _cursorPositionChangePending;
        bool _selectionLengthChangePending;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (Control == null)
                {
                    return;
                }

                if (e.OldElement != null)
                {
                    Control.EditingDidBegin -= Control_EditingDidBegin;
                }

                if (e.NewElement != null)
                {
                    var entry = (CustomEntry)e.NewElement;

                    if (!entry.RemovePadding)
                    {
                        ResizeHeight();
                    }

                    switch (entry.Capitalization)
                    {
                        case CapitalizationType.None:
                            this.Control.AutocapitalizationType = UITextAutocapitalizationType.None;
                            break;
                        case CapitalizationType.Characters:
                            this.Control.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
                            break;
                        case CapitalizationType.Sentences:
                            this.Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
                            break;
                        case CapitalizationType.Words:
                            this.Control.AutocapitalizationType = UITextAutocapitalizationType.Words;
                            break;
                        default:
                            this.Control.AutocapitalizationType = UITextAutocapitalizationType.None;
                            break;
                    }

                    if (entry.SelectAll)
                    {
                        Control.EditingDidBegin += Control_EditingDidBegin;
                    }

                    switch (entry.HorizontalTextAlignment)
                    {
                        case TextAlignment.Start:
                            Control.TextAlignment = UITextAlignment.Left;
                            break;
                        case TextAlignment.Center:
                            Control.TextAlignment = UITextAlignment.Center;
                            break;
                        case TextAlignment.End:
                            Control.TextAlignment = UITextAlignment.Right;
                            break;
                        default:
                            Control.TextAlignment = UITextAlignment.Left;
                            break;
                    }


                    if (entry.HasPrevious && entry.HasNext && entry.HasDone)
                    {
                        UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.Frame.Size.Width, 44.0f)) { Translucent = true };
                        toolbar.Items = new[]
                        {
                    new UIBarButtonItem("Prev", UIBarButtonItemStyle.Plain, delegate {entry.InvokePrevious(); }),
                    new UIBarButtonItem("Next", UIBarButtonItemStyle.Plain, delegate {entry.InvokeNext(); }),
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {Control.ResignFirstResponder(); })
                };
                        this.Control.InputAccessoryView = toolbar;
                    }
                    else if (entry.HasPrevious && entry.HasDone)
                    {
                        UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.Frame.Size.Width, 44.0f)) { Translucent = true };
                        toolbar.Items = new[]
                        {
                    new UIBarButtonItem("Prev", UIBarButtonItemStyle.Plain, delegate {entry.InvokePrevious(); }),
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {Control.ResignFirstResponder(); })
                };
                        this.Control.InputAccessoryView = toolbar;
                    }
                    else if (entry.HasNext && entry.HasDone)
                    {
                        UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.Frame.Size.Width, 44.0f)) { Translucent = true };
                        toolbar.Items = new[]
                        {
                    new UIBarButtonItem("Next", UIBarButtonItemStyle.Plain, delegate {entry.InvokeNext(); }),
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {Control.ResignFirstResponder(); })
                };
                        this.Control.InputAccessoryView = toolbar;
                    }
                    else if (entry.HasPrevious)
                    {
                        UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.Frame.Size.Width, 44.0f)) { Translucent = true };
                        toolbar.Items = new[]
                        {
                    new UIBarButtonItem("Prev", UIBarButtonItemStyle.Plain, delegate {entry.InvokePrevious(); })
                };
                        this.Control.InputAccessoryView = toolbar;
                    }
                    else if (entry.HasNext)
                    {
                        UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.Frame.Size.Width, 44.0f)) { Translucent = true };
                        toolbar.Items = new[]
                        {
                    new UIBarButtonItem("Next", UIBarButtonItemStyle.Plain, delegate {entry.InvokeNext(); })
                };
                        this.Control.InputAccessoryView = toolbar;
                    }
                    else if (entry.HasDone)
                    {
                        UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.Frame.Size.Width, 44.0f)) { Translucent = true };
                        toolbar.Items = new[]
                        {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {Control.ResignFirstResponder(); })
                };
                        this.Control.InputAccessoryView = toolbar;
                    }

                    SetReturnType(entry);

                    if (entry.ReturnType == CustomControls.ReturnType.Go || entry.ReturnType == CustomControls.ReturnType.Search || entry.ReturnType == CustomControls.ReturnType.Send)
                    {
                        Control.ShouldReturn += (UITextField tf) =>
                        {
                            entry.InvokeGoTo();
                            return true;
                        };
                    }
                    else if (entry.ReturnType == CustomControls.ReturnType.Next)
                    {
                        Control.ShouldReturn += (UITextField tf) =>
                        {
                            entry.InvokeNext();
                            return false;
                        };
                    }
                    else if (entry.ReturnType == CustomControls.ReturnType.Done)
                    {
                        Control.ShouldReturn += (UITextField tf) =>
                        {
                            return true;
                        };
                    }

                    Control.Font = UIFont.SystemFontOfSize((nfloat)entry.FontSize);
                    Control.BorderStyle = entry.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;

                    SetNativeControl(Control);

                    //This fix loosing focus.
                    _cursorPositionChangePending = Element.IsSet(Entry.CursorPositionProperty);
                    _selectionLengthChangePending = Element.IsSet(Entry.SelectionLengthProperty);

                    if (_cursorPositionChangePending || _selectionLengthChangePending)
                    {
                        UpdateCursorSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        //This fix loosing focus.
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Entry.CursorPositionProperty.PropertyName)
            {
                UpdateCursorSelection();
            }
            else if (e.PropertyName == Entry.SelectionLengthProperty.PropertyName)
            {
                UpdateCursorSelection();
            }

            if (!((CustomEntry)sender).RemovePadding)
            {
                ResizeHeight();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void ResizeHeight()
        {
            if (Element.HeightRequest >= 0)
            {
                return;
            }

            var height = Math.Max(Bounds.Height,
                new UITextField { Font = Control.Font }.IntrinsicContentSize.Height);

            Control.Frame = new CoreGraphics.CGRect(0.0f, 0.0f, (nfloat)Element.Width, (nfloat)height);

            Element.HeightRequest = height;
        }

        private void Control_EditingDidBegin(object sender, EventArgs e)
        {
            ((UITextField)Control).PerformSelector(new Selector("selectAll"), null, 0.0f);

            if (!_cursorPositionChangePending && !_selectionLengthChangePending)
            {
                UpdateCursorFromControl(null);
            }
            else
            {
                UpdateCursorSelection();
            }
        }

        void UpdateCursorSelection()
        {
            if (_nativeSelectionIsUpdating || Control == null || Element == null)
            {
                return;
            }

            _cursorPositionChangePending = _selectionLengthChangePending = true;

            // If this is run from the ctor, the control is likely too early in its lifecycle to be first responder yet. 
            // Anything done here will have no effect, so we'll skip this work until later.
            // We'll try again when the control does become first responder later OnEditingBegan
            if (Control.BecomeFirstResponder())
            {
                try
                {
                    int cursorPosition = Element.CursorPosition;

                    UITextPosition start = GetSelectionStart(cursorPosition, out int startOffset);
                    UITextPosition end = GetSelectionEnd(cursorPosition, start, startOffset);

                    Control.SelectedTextRange = Control.GetTextRange(start, end);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Entry", $"Failed to set Control.SelectedTextRange from CursorPosition/SelectionLength: {ex}");
                }
                finally
                {
                    _cursorPositionChangePending = _selectionLengthChangePending = false;
                }
            }
        }

        void UpdateCursorFromControl(NSObservedChange obj)
        {
            if (_nativeSelectionIsUpdating || Control == null || Element == null)
            {
                return;
            }

            var currentSelection = Control.SelectedTextRange;
            if (currentSelection != null)
            {
                if (!_cursorPositionChangePending)
                {
                    int newCursorPosition = (int)Control.GetOffsetFromPosition(Control.BeginningOfDocument, currentSelection.Start);
                    if (newCursorPosition != Element.CursorPosition)
                    {
                        SetCursorPositionFromRenderer(newCursorPosition);
                    }
                }

                if (!_selectionLengthChangePending)
                {
                    int selectionLength = (int)Control.GetOffsetFromPosition(currentSelection.Start, currentSelection.End);

                    if (selectionLength != Element.SelectionLength)
                    {
                        SetSelectionLengthFromRenderer(selectionLength);
                    }
                }
            }
        }

        UITextPosition GetSelectionEnd(int cursorPosition, UITextPosition start, int startOffset)
        {
            UITextPosition end = start;
            int endOffset = startOffset;
            int selectionLength = Element.SelectionLength;

            if (Element.IsSet(Entry.SelectionLengthProperty))
            {
                end = Control.GetPosition(start, Math.Max(startOffset, Math.Min(Control.Text.Length - cursorPosition, selectionLength))) ?? start;
                endOffset = Math.Max(startOffset, (int)Control.GetOffsetFromPosition(Control.BeginningOfDocument, end));
            }

            int newSelectionLength = Math.Max(0, endOffset - startOffset);
            if (newSelectionLength != selectionLength)
            {
                SetSelectionLengthFromRenderer(newSelectionLength);
            }

            return end;
        }

        UITextPosition GetSelectionStart(int cursorPosition, out int startOffset)
        {
            UITextPosition start = Control.EndOfDocument;
            startOffset = Control.Text.Length;

            if (Element.IsSet(Entry.CursorPositionProperty))
            {
                start = Control.GetPosition(Control.BeginningOfDocument, cursorPosition) ?? Control.EndOfDocument;
                startOffset = Math.Max(0, (int)Control.GetOffsetFromPosition(Control.BeginningOfDocument, start));
            }

            if (startOffset != cursorPosition)
            {
                SetCursorPositionFromRenderer(startOffset);
            }

            return start;
        }

        void SetSelectionLengthFromRenderer(int selectionLength)
        {
            try
            {
                _nativeSelectionIsUpdating = true;
                Element?.SetValueFromRenderer(Entry.SelectionLengthProperty, selectionLength);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Entry", $"Failed to set SelectionLength from renderer: {ex}");
            }
            finally
            {
                _nativeSelectionIsUpdating = false;
            }
        }

        void SetCursorPositionFromRenderer(int start)
        {
            try
            {
                _nativeSelectionIsUpdating = true;
                Element?.SetValueFromRenderer(Entry.CursorPositionProperty, start);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Entry", $"Failed to set CursorPosition from renderer: {ex}");
            }
            finally
            {
                _nativeSelectionIsUpdating = false;
            }
        }
        private void SetReturnType(CustomEntry entry)
        {
            CustomControls.ReturnType type = entry.ReturnType;

            switch (type)
            {
                case CustomControls.ReturnType.Go:
                    Control.ReturnKeyType = UIReturnKeyType.Go;
                    break;
                case CustomControls.ReturnType.Next:
                    Control.ReturnKeyType = UIReturnKeyType.Next;
                    break;
                case CustomControls.ReturnType.Previous:
                    Control.ReturnKeyType = UIReturnKeyType.Default;
                    break;
                case CustomControls.ReturnType.Send:
                    Control.ReturnKeyType = UIReturnKeyType.Send;
                    break;
                case CustomControls.ReturnType.Search:
                    Control.ReturnKeyType = UIReturnKeyType.Search;
                    break;
                case CustomControls.ReturnType.Done:
                    Control.ReturnKeyType = UIReturnKeyType.Done;
                    break;
                default:
                    Control.ReturnKeyType = UIReturnKeyType.Default;
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}