using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using PoolGuy.Mobile.Data.Models;
using System.Runtime.CompilerServices;

namespace PoolGuy.Mobile.CustomControls
{
    public class NavigationGrid : Grid
    {
        #region Constans
        // Page/icon
        
        Dictionary<string, string> pages =
            new Dictionary<string, string> {
                { "Home", "dashboard_black.png" },
                { "Customer", "account_circle_black.png" },
                { "Scheduler", "account_circle_black.png" },
            };
        #endregion
        #region Binding Properties
        public static BindableProperty CurrenProperty =
           BindableProperty.Create(nameof(Current), typeof(string), typeof(NavigationGrid), string.Empty, BindingMode.OneWay);

        public string Current
        {
            get => (string)GetValue(CurrenProperty);
            set => SetValue(CurrenProperty, value);
        }

        public static BindableProperty CommandProperty =
           BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(NavigationGrid), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        #endregion

        public static BindableProperty SelectedColorProperty =
           BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(NavigationGrid), Color.FromHex("#007cdc"), BindingMode.OneWay);

        /// <summary>
        /// Paint current selected page
        /// </summary>
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static BindableProperty UnSelectedColorProperty =
   BindableProperty.Create(nameof(UnSelectedColor), typeof(Color), typeof(NavigationGrid), Color.FromHex("#858585"), BindingMode.OneWay);
        /// <summary>
        /// Paint current unselected page
        /// </summary>
        public Color UnSelectedColor
        {
            get => (Color)GetValue(UnSelectedColorProperty);
            set => SetValue(UnSelectedColorProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            if (propertyName == CurrenProperty.PropertyName && Children != null)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            // Clear
            Children?.Clear();

            // Add Row Definitions
            AddRowDefinitions(1);

            // Add Columns Definitions
            AddColumnsDefinitions(pages.Count);

            // Add cells
            AddCells(pages.Count);
        }

        private void AddRowDefinitions(int rows)
        {
            RowDefinitionCollection rd = new RowDefinitionCollection();
            for (int i = 0; i < rows; i++)
            {
                rd.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            this.RowDefinitions = rd;
        }

        private void AddColumnsDefinitions(int columns)
        {
            ColumnDefinitionCollection cd = new ColumnDefinitionCollection();
            for (int i = 0; i < columns; i++)
            {
                cd.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            this.ColumnDefinitions = cd;
        }

        private void AddCells(int columns)
        {
            for (int i = 0; i < columns; i++)
            {
                AnimatedButton animatedButton = new AnimatedButton()
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    TouchUpCommand = Command,
                };

                var page = pages.ElementAt(i);
                StackLayout stack = new StackLayout()
                {
                    Spacing = 2,
                    Padding = new Thickness(0, 6)
                };

                switch (page.Key)
                {
                    case "Home":
                        animatedButton.TouchDownCommandParameter = Enums.ePage.Home;
                        break;
                    case "Customer":
                        animatedButton.TouchDownCommandParameter = Enums.ePage.SearchCustomer;
                        break;
                    case "Scheduler":
                        animatedButton.TouchDownCommandParameter = Enums.ePage.Scheduler;
                        break;
                    default:
                        break;
                }

                // Add stacklayout
                

                AbsoluteLayout.SetLayoutFlags(stack, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(stack, new Rectangle(.5f, .5f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

                // Add icon source
                ColorImage icon = new ColorImage()
                {
                    Source = page.Value,
                    Foreground = i == pages.ToList().FindIndex(x => x.Key == Current) ? SelectedColor : UnSelectedColor,
                    HeightRequest = 30,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start
                };

                // Add icon to stacklayout
                stack.Children.Add(icon);

                Label heading = new Label()
                {
                    Text = page.Key,
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = i == pages.ToList().FindIndex(x => x.Key == Current) ? SelectedColor : UnSelectedColor
                };

                // Add heading label to stack
                stack.Children.Add(heading);

                // Add stack to animate button
                animatedButton.Children.Add(stack);

                // Add animate button to grid
                Children?.Add(animatedButton, i , 0);
            }
        }
    }
}