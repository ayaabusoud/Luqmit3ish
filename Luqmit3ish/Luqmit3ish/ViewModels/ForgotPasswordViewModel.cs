using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{

    class ForgotPasswordViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand SendEmailCommand { protected set; get; }
        public ICommand LoginCommand { protected set; get; }

        public ForgotPasswordViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            SendEmailCommand = new Command(OnSendEmailClicked);
            LoginCommand = new Command(OnLoginClicked);
        }
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private void OnSendEmailClicked()
        {
            try
            {
                Application.Current.MainPage = new CheckEmailPage(Email);

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
        private void OnLoginClicked()
        {
            try
            {
                Application.Current.MainPage = new LoginPage();
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