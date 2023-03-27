using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class VerificationViewModel 
    {
        public Command SignupCommand { get; }


        public VerificationViewModel()
        {
            SignupCommand = new Command(OnSignupClicked);
        }

        private void OnSignupClicked(object obj)
        {
            Application.Current.MainPage  = new AppShell();
        }
    }
}
