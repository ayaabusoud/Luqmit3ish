using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class VerificationViewModel  : ViewModelBase
    {
        public Command SignupCommand { get; }


        public VerificationViewModel()
        {
            SignupCommand = new Command(OnSignupClicked);
        }

        private void OnSignupClicked()
        {
            try
            {
            Application.Current.MainPage  = new AppShellCharity();

            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
