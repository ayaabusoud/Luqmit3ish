using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ForgotPassCommand { get; }
        public ICommand SignupCommand { get; }
        public ICommand LoginCommand {  get; }
        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            ForgotPassCommand = new Command(() => OnForgotPassClicked());
            SignupCommand = new Command(() => OnSignupClicked());
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnForgotPassClicked()
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }
        private async void OnSignupClicked()
        {
            await Navigation.PushModalAsync(new SignupPage());
        }

        private void OnLoginClicked()
        {
              Application.Current.MainPage = new AppShell();
        }
    }
}

