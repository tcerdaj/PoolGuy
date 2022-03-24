using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.CustomControls
{
    public class AdjustableEditor : Editor
    {
        public bool HasBorder { get; set; } = false;
        // This is used for locking the scrolling ability of the Editor in iOS 
        public bool ScrollEnabled { get; set; } = false;

        public static readonly BindableProperty HasDoneProperty =
          BindableProperty.Create(nameof(HasDone), typeof(bool), typeof(AdjustableEditor), false);

        public bool HasDone
        {
            get { return (bool)this.GetValue(HasDoneProperty); }
            set { this.SetValue(HasDoneProperty, value); }
        }

        public static readonly BindableProperty HasCancelProperty =
         BindableProperty.Create(nameof(HasCancel), typeof(bool), typeof(AdjustableEditor), false);

        public bool HasCancel
        {
            get { return (bool)this.GetValue(HasCancelProperty); }
            set { this.SetValue(HasCancelProperty, value); }
        }

        #region Done
        public event EventHandler Done;

        public static BindableProperty DoneCommandProperty =
            BindableProperty.Create(nameof(DoneCommand), typeof(ICommand), typeof(AdjustableEditor), null);

        public ICommand DoneCommand
        {
            get { return (ICommand)GetValue(DoneCommandProperty); }
            set { SetValue(DoneCommandProperty, value); }
        }

        public static BindableProperty DoneCommandParameterProperty =
            BindableProperty.Create(nameof(DoneCommandParameter), typeof(object), typeof(AdjustableEditor), null);

        public object DoneCommandParameter
        {
            get { return (object)GetValue(DoneCommandParameterProperty); }
            set { SetValue(DoneCommandParameterProperty, value); }
        }

        public void InvokeDone()
        {
            if (Done != null)
            {
                Done.Invoke(this, null);
            }
            else if (DoneCommand != null)
            {
                DoneCommand.Execute(DoneCommandParameter);
            }
        }

        #endregion

        /// <summary>
        /// Removes the padding from Android Editor to make it like the iOS Editor which has no padding.
        /// </summary>
        public static readonly BindableProperty RemovePaddingProperty =
            BindableProperty.Create(nameof(RemovePadding), typeof(bool), typeof(AdjustableEditor), false);

        public bool RemovePadding
        {
            get { return (bool)GetValue(RemovePaddingProperty); }
            set { SetValue(RemovePaddingProperty, value); }
        }
    }
}