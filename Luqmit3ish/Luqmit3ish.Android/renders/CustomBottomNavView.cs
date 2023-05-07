using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Luqmit3ish.Droid.renders
{
    public class CustomBottomNavView : IShellBottomNavViewAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(BottomNavigationView bottomView)
        {

        }


        public void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            


            // Get the current theme mode (light or dark)
            var currentNightMode = Preferences.Get("DarkTheme", false);

            // Set the color for the unselected tab based on the current mode
            if (!currentNightMode)
            {
                bottomView.SetBackgroundColor(Android.Graphics.Color.White);

            }
            else
            {
                bottomView.SetBackgroundColor(Android.Graphics.Color.ParseColor("#212121"));

            }
        }
    }
}