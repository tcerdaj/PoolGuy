using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.iOS.CustomRenderer;
using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRendererAttribute(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace PoolGuy.Mobile.iOS.CustomRenderer
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            if (e.OldElement != null)
            {
                this.Control.Started -= Control_Started;
            }

            if (e.NewElement != null)
            {
                this.AddButtons();
                this.Control.BorderStyle = UITextBorderStyle.Line;
                Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                Control.Layer.BorderWidth = 1;
                this.Control.Started += Control_Started;

                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    this.Control.Font = UIFont.SystemFontOfSize(25);
                }
            }
        }

        private void Control_Started(object sender, EventArgs e)
        {
            CustomDatePicker baseDatePicker = this.Element as CustomDatePicker;
            baseDatePicker._originalNullableDate = baseDatePicker.NullableDate;
        }

        private void AddButtons()
        {
            var originalToolbar = this.Control.InputAccessoryView as UIToolbar;
            if (originalToolbar != null && originalToolbar.Items.Length <= 2)
            {
                CustomDatePicker baseDatePicker = this.Element as CustomDatePicker;
                var clearButton = new UIBarButtonItem("Remove Date", UIBarButtonItemStyle.Plain, ((sender, ev) =>
                {
                    this.Element.Unfocus();
                    this.Element.Date = DateTime.Now;
                    baseDatePicker.CleanDate();
                }));

                var cancelButton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, ((sender, ev) =>
                {
                    baseDatePicker._originalNullableDate = baseDatePicker.NullableDate;
                    this.Element.Unfocus();
                    baseDatePicker.Cancel();
                }));

                var newItems = new List<UIBarButtonItem>();
                foreach (var item in originalToolbar.Items)
                {
                    newItems.Add(item);
                }

                if (baseDatePicker.ShowClearButton)
                {
                    newItems.Insert(0, clearButton);
                    newItems.Insert(1, cancelButton);
                }
                else
                {
                    newItems.Insert(0, cancelButton);
                }

                originalToolbar.Items = newItems.ToArray();
                originalToolbar.SetNeedsDisplay();
            }
        }
    }
}