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
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Routing.RegisterRoute(nameof(LocationPage), typeof(LocationPage));
            Routing.RegisterRoute(nameof(VerificationPage), typeof(VerificationPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            //delete code
        }
        private async void OnDarkModeClicked(object sender, EventArgs e)
        {
            //dark mode code
        }
    }
}
