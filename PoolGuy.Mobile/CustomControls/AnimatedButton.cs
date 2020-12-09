using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoolGuy.Mobile.CustomControls
{
    public class AnimatedButton : MR.Gestures.AbsoluteLayout
    {
        private RoundedBoxView roundedBox;

        public AnimatedButton()
        {
            DownCommand = new Command(AnimatedButton_Down);
            UpCommand = new Command(AnimatedButton_Up);
            //this.Down += AnimatedButton_Down; 
            //this.Up += AnimatedButton_Up;
        }

        //private void AnimatedButton_Down(object sender, MR.Gestures.DownUpEventArgs e)
        private void AnimatedButton_Down()
        {
            if (!Enabled)
            {
                return;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                await this.ScaleTo(0.90, 50, Easing.CubicOut);
                if (TouchDown != null)
                {
                    TouchDown(this, EventArgs.Empty);
                }
                else if (TouchDownCommand != null)
                {
                    TouchDownCommand.Execute(TouchDownCommandParameter);
                }
            });
        }

        //private void AnimatedButton_Up(object sender, MR.Gestures.DownUpEventArgs e)
        private void AnimatedButton_Up()
        {
            if (!Enabled)
            {
                return;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                await this.ScaleTo(1, 50, Easing.CubicIn);
                if (TouchUp != null)
                {
                    TouchUp(this, EventArgs.Empty);
                }
                else if (TouchUpCommand != null)
                {
                    TouchUpCommand.Execute(TouchUpCommandParameter);
                }
            });
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            roundedBox = new RoundedBoxView()
            {
                BackgroundColor = ButtonColor,
                CornerRadius = CornerRadius,
                BorderColor = ButtonBorderColor,
                BorderThickness = ButtonBorderThickness,
            };
            AbsoluteLayout.SetLayoutFlags(roundedBox, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(roundedBox, new Rectangle(0, 0, 1, 1));
            Children.Insert(0, roundedBox);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }
            else if (propertyName == ButtonColorProperty.PropertyName)
            {
                if (roundedBox != null)
                {
                    roundedBox.BackgroundColor = ButtonColor;
                }
            }
            else if (propertyName == ButtonBorderColorProperty.PropertyName)
            {
                if (roundedBox != null)
                {
                    roundedBox.BorderColor = ButtonBorderColor;
                }
            }
            else if (propertyName == ButtonBorderThicknessProperty.PropertyName)
            {
                if (roundedBox != null)
                {
                    roundedBox.BorderThickness = ButtonBorderThickness;
                }
            }
            else if (propertyName == CornerRadiusProperty.PropertyName)
            {
                if (roundedBox != null)
                {
                    roundedBox.CornerRadius = CornerRadius;
                }
            }
        }

        /// <summary>
        /// This Color property sets the color of the button
        /// </summary>
        public static BindableProperty ButtonColorProperty =
        BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(AnimatedButton), Color.Transparent);
        //public static BindableProperty ButtonColorProperty =
        //BindableProperty.Create<AnimatedButton, Color>(x => x.ButtonColor, Color.Transparent);

        public Color ButtonColor
        {
            get => (Color)GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }

        /// <summary>
        /// This Color property sets the color of the button border
        /// </summary>
        public static BindableProperty ButtonBorderColorProperty =
        BindableProperty.Create(nameof(ButtonBorderColor), typeof(Color), typeof(AnimatedButton), Color.Transparent);
        //public static BindableProperty ButtonBorderColorProperty =
        //BindableProperty.Create<AnimatedButton, Color>(x => x.ButtonBorderColor, Color.Transparent);

        public Color ButtonBorderColor
        {
            get => (Color)GetValue(ButtonBorderColorProperty);
            set => SetValue(ButtonBorderColorProperty, value);
        }

        /// <summary>
        /// This int property sets the corner radius of the button
        /// </summary>
        public static BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(AnimatedButton), 0);
        //public static BindableProperty CornerRadiusProperty =
        //BindableProperty.Create<AnimatedButton, int>(x => x.CornerRadius, 0);

        public int CornerRadius
        {
            get => (int)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// This int property sets the width of the button border
        /// </summary>
        public static BindableProperty ButtonBorderThicknessProperty =
        BindableProperty.Create(nameof(ButtonBorderThickness), typeof(int), typeof(AnimatedButton), 0);
        //public static BindableProperty ButtonBorderThicknessProperty =
        //BindableProperty.Create<AnimatedButton, int>(x => x.ButtonBorderThickness, 0);

        public int ButtonBorderThickness
        {
            get => (int)GetValue(ButtonBorderThicknessProperty);
            set => SetValue(ButtonBorderThicknessProperty, value);
        }

        /// <summary>
        /// This event handler is used to run the passed in event when the button is pressed down
        /// </summary>
        public event EventHandler TouchDown;

        /// <summary>
        /// This command gets executed on Touch Down Event right after the Scale Down Animation
        /// This is ideal to start the data fetching process before navigation
        /// </summary>
        public static BindableProperty TouchDownCommandProperty =
        BindableProperty.Create(nameof(TouchDownCommand), typeof(ICommand), typeof(AnimatedButton), null);
        //public static BindableProperty TouchDownCommandProperty =
        //BindableProperty.Create<AnimatedButton, ICommand>(x => x.TouchDownCommand, null);

        public ICommand TouchDownCommand
        {
            get => (ICommand)GetValue(TouchDownCommandProperty);
            set => SetValue(TouchDownCommandProperty, value);
        }

        /// <summary>
        /// This object will be passed in when the TouchDownCommand is executed
        /// </summary>
        public static BindableProperty TouchDownCommandParameterProperty =
        BindableProperty.Create(nameof(TouchDownCommandParameter), typeof(object), typeof(AnimatedButton), null);
        //public static BindableProperty TouchDownCommandParameterProperty =
        //BindableProperty.Create<AnimatedButton, object>(x => x.TouchDownCommandParameter, null);

        public object TouchDownCommandParameter
        {
            get => GetValue(TouchDownCommandParameterProperty);
            set => SetValue(TouchDownCommandParameterProperty, value);
        }

        /// <summary>
        /// This event handler is used to run the passend in event when the button is released
        /// </summary>
        public event EventHandler TouchUp;

        /// <summary>
        /// This command get executed on Touch Up Event right after the Scale Up Animation
        /// This is ideal to start the navigation process
        /// </summary>
        public static BindableProperty TouchUpCommandProperty =
        BindableProperty.Create(nameof(TouchUpCommand), typeof(ICommand), typeof(AnimatedButton), null);
        //public static BindableProperty TouchUpCommandProperty =
        //BindableProperty.Create<AnimatedButton, ICommand>(x => x.TouchUpCommand, null);

        public ICommand TouchUpCommand
        {
            get => (ICommand)GetValue(TouchUpCommandProperty);
            set => SetValue(TouchUpCommandProperty, value);
        }

        /// <summary>
        /// This object will be passed in when the TouchUpCommand is executed
        /// </summary>
        public static BindableProperty TouchUpCommandParameterProperty =
        BindableProperty.Create(nameof(TouchUpCommandParameter), typeof(object), typeof(AnimatedButton), null);
        //public static BindableProperty TouchUpCommandParameterProperty =
        //BindableProperty.Create<AnimatedButton, object>(x => x.TouchUpCommandParameter, null);

        public object TouchUpCommandParameter
        {
            get => GetValue(TouchUpCommandParameterProperty);
            set => SetValue(TouchUpCommandParameterProperty, value);
        }


        /// <summary>
        /// This bool value determines whether the button is allowed to perform the animation and execute the commends
        /// </summary>
        public static BindableProperty EnabledProperty =
        BindableProperty.Create(nameof(Enabled), typeof(bool), typeof(AnimatedButton), true);
        //public static BindableProperty EnabledProperty =
        //BindableProperty.Create<AnimatedButton, bool>(x => x.Enabled, true);

        public bool Enabled
        {
            get => (bool)GetValue(EnabledProperty);
            set => SetValue(EnabledProperty, value);
        }
    }
}
