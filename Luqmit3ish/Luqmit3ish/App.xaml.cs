using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luqmit3ish
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new OnBoardingPage());
        }

        protected override void OnStart()
        {
            RemovePreferences();
        }

        protected override void OnSleep()
        {
            RemovePreferences();
        }

        protected override void OnResume()
        {
            RemovePreferences();
        }

        private void RemovePreferences()
        {
            Preferences.Remove("FilteedDishes");
            Preferences.Remove("LowerKeepValid");
            Preferences.Remove("UpperKeepValid");
            Preferences.Remove("LowerQuantity");
            Preferences.Remove("UpperQuantity");
            Preferences.Remove("SelectedTypeValues");
            Preferences.Remove("SelectedLocationValues");
        }
    }
}
