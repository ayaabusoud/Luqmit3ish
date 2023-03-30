using Luqmit3ish.ViewModels;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Luqmit3ish
{
    public partial class AppShellCharity : Xamarin.Forms.Shell
    {
        public AppShellCharity()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CharityHomePage), typeof(CharityHomePage));
            Routing.RegisterRoute(nameof(CharityOrderPage), typeof(CharityOrderPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));

        }
    }
}
