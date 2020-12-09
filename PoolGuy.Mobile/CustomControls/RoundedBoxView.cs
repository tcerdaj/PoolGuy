using Xamarin.Forms;

namespace PoolGuy.Mobile.CustomControls
{
    public class RoundedBoxView : BoxView
    {
        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", typeof(double), typeof(RoundedBoxView), 0.0);

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// The border color property
        /// </summary>
        //public static readonly BindableProperty BorderColorProperty =
        //    BindableProperty.Create<RoundedBoxView, Color>(p => p.BorderColor, Color.Transparent);
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RoundedBoxView), Color.Transparent);

        /// <summary>
        /// Get or Sets the Border Color
        /// </summary>
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        /// <summary>
        /// The border thickness property
        /// </summary>
        //public static readonly BindableProperty BorderThicknessProperty =
        //    BindableProperty.Create<RoundedBoxView, double>(p => p.BorderThickness, default(double));
        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(RoundedBoxView), default(double));

        /// <summary>
        /// Gets or Sets the Border Thickness
        /// </summary>
        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        /// <summary>
        /// The shadow property
        /// </summary>
        //public static readonly BindableProperty HasShadowProperty = 
        //    BindableProperty.Create<RoundedBoxView, bool>(p => p.HasShadow, default(bool));
        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(RoundedBoxView), default(bool));

        /// <summary>
        /// Get or Sets whether there is shadow or not
        /// </summary>
        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }
    }
}
