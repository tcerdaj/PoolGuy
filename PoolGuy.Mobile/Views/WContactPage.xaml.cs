﻿using CommonServiceLocator;
using PoolGuy.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolGuy.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WContactPage : ContentPage
    {
        CustomerViewModel _viewModel;
        public WContactPage()
        {
            InitializeComponent();
        }
    }
}