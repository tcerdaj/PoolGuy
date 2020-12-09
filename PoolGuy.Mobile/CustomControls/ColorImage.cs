
using Xamarin.Forms;

namespace PoolGuy.Mobile.CustomControls
{
    public class ColorImage : Xamarin.Forms.View
    {
        /// <summary>
        /// The color fill for the image
        /// </summary>
        //public static readonly BindableProperty ForegroundProperty = 
        //BindableProperty.Create<ColorImage, Color>(p => p.Foreground, default(Color));
        public static readonly BindableProperty ForegroundProperty =
        BindableProperty.Create(nameof(Foreground), typeof(Color), typeof(ColorImage), default(Color));

        public Color Foreground
        {
            get => (Color)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }


        /// <summary>
        /// The image source
        /// </summary>
        //public static readonly BindableProperty SourceProperty = 
        //BindableProperty.Create<ColorImage, string>(p => p.Source, default(string));
        public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(string), typeof(ColorImage), default(string));

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
    }
}
