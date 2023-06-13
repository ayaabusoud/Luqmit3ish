using Luqmit3ish.ViewModels;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Luqmit3ish
{

        public partial class AppShellRestaurant : Xamarin.Forms.Shell
        {
            public AppShellRestaurant()
            {
                InitializeComponent();
                Routing.RegisterRoute(nameof(RestaurantHomePage), typeof(RestaurantHomePage));
                Routing.RegisterRoute(nameof(RestaurantOrderPage), typeof(RestaurantOrderPage));
                Routing.RegisterRoute(nameof(settingsPage), typeof(settingsPage));

            }
        }
    
}
