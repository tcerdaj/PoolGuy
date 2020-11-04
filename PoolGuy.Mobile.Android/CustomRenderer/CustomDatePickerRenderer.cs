using System;
using Android.App;
using Android.Content;
using Android.Widget;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    internal class CustomDatePickerRenderer : ViewRenderer<CustomDatePicker, EditText>
    {

        DatePickerDialog _dialog;

        public CustomDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomDatePicker> e)
        {
            base.OnElementChanged(e);
            //this.SetNativeControl(new Android.Widget.EditText(Android.App.Application.Context));
            this.SetNativeControl(new Android.Widget.EditText(this.Context));

            if (Control == null)
            {
                return;
            }

            if (e.OldElement != null)
            {
                this.Control.Click -= OnPickerClick;
            }

            if (e.NewElement != null)
            {
                this.Control.Click += OnPickerClick;
                this.Control.Text = Element.Date.ToString(Element.Format);
                this.Control.KeyListener = null;
                this.Control.Enabled = Element.IsEnabled;
            }
        }

        private void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        protected override void OnFocusChangeRequested(object sender, VisualElement.FocusRequestArgs e)
        {
            base.OnFocusChangeRequested(sender, e);
            if (e.Focus)
            {
                ShowDatePicker();
            }
        }
        private void ShowDatePicker()
        {
            CreateDatePickerDialog(this.Element.Date.Year, this.Element.Date.Month - 1, this.Element.Date.Day);
            this.Element._originalNullableDate = this.Element.NullableDate;
            _dialog.Show();
        }

        void CreateDatePickerDialog(int year, int month, int day)
        {
            CustomDatePicker view = Element;
            _dialog = new DatePickerDialog(Context, callBack: (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, year: year, monthOfYear: month, dayOfMonth: day);

            //Show Done button by default
            _dialog.SetButton("Done", (sender, e) =>
            {
                SetDate(_dialog.DatePicker.DateTime);
                this.Element.Format = this.Element._originalFormat;
                this.Element.AssignValue();
            });
            if (this.Element.ShowClearButton)
            {
                _dialog.SetButton2("Cancel", (sender, e) =>
                {
                    this.Element.Cancel();
                    Control.Text = this.Element.Format;
                });

                _dialog.SetButton3("Remove Date", (sender, e) =>
                {
                    this.Element.CleanDate();
                    Control.Text = this.Element.Format;
                });
            }
            else  //Show Cancel button by default
            {
                _dialog.SetButton2("Cancel", (sender, e) =>
                {
                    this.Element.Cancel();
                    Control.Text = this.Element.Format;
                });
            }
        }

        void SetDate(DateTime date)
        {
            this.Control.Text = date.ToString(Element.Format);
            Element.Date = date;
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}