using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }


        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private void OnLoginClicked(object obj)
        {
            Application.Current.MainPage = new AppShell();
        }
 
    }
}
