using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.CustomControls
{
    public class CustomEntry : Entry
    {

        public event EventHandler SelectAllTextEvent;

        public void SelectAllText()
        {
            SelectAllTextEvent?.Invoke(this, EventArgs.Empty);
        }

        // Previous
        public event EventHandler Previous;

        public static BindableProperty PreviousCommandProperty =
            BindableProperty.Create(nameof(PreviousCommand), typeof(ICommand), typeof(CustomEntry), null);

        public ICommand PreviousCommand
        {
            get { return (ICommand)GetValue(PreviousCommandProperty); }
            set { SetValue(PreviousCommandProperty, value); }
        }

        public static BindableProperty PreviousCommandParameterProperty =
            BindableProperty.Create(nameof(PreviousCommandParameter), typeof(object), typeof(CustomEntry), null);

        public object PreviousCommandParameter
        {
            get { return (object)GetValue(PreviousCommandParameterProperty); }
            set { SetValue(PreviousCommandParameterProperty, value); }
        }

        public void InvokePrevious()
        {
            if (this.Previous != null)
            {
                this.Previous.Invoke(this, null);
            }
            else if (this.PreviousCommand != null)
            {
                this.PreviousCommand.Execute(PreviousCommandParameter);
            }
        }

        // Next
        public event EventHandler Next;

        public static BindableProperty NextCommandProperty =
            BindableProperty.Create(nameof(NextCommand), typeof(ICommand), typeof(CustomEntry), null);

        public ICommand NextCommand
        {
            get { return (ICommand)GetValue(NextCommandProperty); }
            set { SetValue(NextCommandProperty, value); }
        }

        public static BindableProperty NextCommandParameterProperty =
            BindableProperty.Create(nameof(NextCommandParameter), typeof(object), typeof(CustomEntry), null);

        public object NextCommandParameter
        {
            get { return (object)GetValue(NextCommandParameterProperty); }
            set { SetValue(NextCommandParameterProperty, value); }
        }

        public void InvokeNext()
        {
            if (this.Next != null)
            {
                this.Next.Invoke(this, null);
            }
            else if (this.NextCommand != null)
            {
                this.NextCommand.Execute(NextCommandParameter);
            }
        }

        // GoTo
        public event EventHandler GoTo;

        public static BindableProperty GoToCommandProperty =
            BindableProperty.Create(nameof(GoToCommand), typeof(ICommand), typeof(CustomEntry), null);

        public ICommand GoToCommand
        {
            get { return (ICommand)GetValue(GoToCommandProperty); }
            set { SetValue(GoToCommandProperty, value); }
        }

        public static BindableProperty GoToCommandParameterProperty =
            BindableProperty.Create(nameof(GoToCommandParameter), typeof(object), typeof(CustomEntry), null);

        public object GoToCommandParameter
        {
            get { return (object)GetValue(GoToCommandParameterProperty); }
            set { SetValue(GoToCommandParameterProperty, value); }
        }

        public void InvokeGoTo()
        {
            if (this.GoTo != null)
            {
                this.GoTo.Invoke(this, null);
            }
            else if (this.GoToCommand != null)
            {
                this.GoToCommand.Execute(GoToCommandParameter);
            }
        }

        public new static readonly BindableProperty ReturnTypeProperty =
            BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(CustomEntry), ReturnType.Default);

        public new ReturnType ReturnType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public static readonly BindableProperty CapitalizationProperty =
            BindableProperty.Create(nameof(Capitalization), typeof(CapitalizationType), typeof(CustomEntry), CapitalizationType.None);

        public CapitalizationType Capitalization
        {
            get { return (CapitalizationType)GetValue(CapitalizationProperty); }
            set { SetValue(CapitalizationProperty, value); }
        }

        public static readonly BindableProperty SelectAllProperty =
            BindableProperty.Create(nameof(SelectAll), typeof(bool), typeof(CustomEntry), false);

        public bool SelectAll
        {
            get { return (bool)this.GetValue(SelectAllProperty); }
            set { this.SetValue(SelectAllProperty, value); }
        }

        public static readonly BindableProperty HasDoneProperty =
            BindableProperty.Create(nameof(HasDone), typeof(bool), typeof(CustomEntry), false);

        public bool HasDone
        {
            get { return (bool)this.GetValue(HasDoneProperty); }
            set { this.SetValue(HasDoneProperty, value); }
        }

        public static readonly BindableProperty HasNextProperty =
            BindableProperty.Create(nameof(HasNext), typeof(bool), typeof(CustomEntry), false);

        public bool HasNext
        {
            get { return (bool)this.GetValue(HasNextProperty); }
            set { this.SetValue(HasNextProperty, value); }
        }

        public static readonly BindableProperty HasPreviousProperty =
            BindableProperty.Create(nameof(HasPrevious), typeof(bool), typeof(CustomEntry), false);

        public bool HasPrevious
        {
            get { return (bool)this.GetValue(HasPreviousProperty); }
            set { this.SetValue(HasPreviousProperty, value); }
        }

        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(CustomEntry), false);

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }

        /// <summary>
        /// Removes the padding from Android Entry to make it like the iOS Entry which has no padding.
        /// </summary>
        public static readonly BindableProperty RemovePaddingProperty =
            BindableProperty.Create(nameof(RemovePadding), typeof(bool), typeof(CustomEntry), false);

        public bool RemovePadding
        {
            get { return (bool)GetValue(RemovePaddingProperty); }
            set { SetValue(RemovePaddingProperty, value); }
        }
    }

    public enum ReturnType
    {
        Default,
        Go,
        Next,
        Previous,
        Done,
        Send,
        Search
    }

    public enum CapitalizationType
    {
        None,
        Characters,
        Sentences,
        Words
    }
}