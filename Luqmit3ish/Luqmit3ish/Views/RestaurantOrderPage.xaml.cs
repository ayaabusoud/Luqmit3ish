﻿using Luqmit3ish.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luqmit3ish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestaurantOrderPage : ContentPage
    {
        public RestaurantOrderPage()
        {
            InitializeComponent();
            this.BindingContext = new RestaurantOrderViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new RestaurantOrderViewModel(Navigation);
        }
    }
}