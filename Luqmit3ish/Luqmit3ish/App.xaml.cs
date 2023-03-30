using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luqmit3ish
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
