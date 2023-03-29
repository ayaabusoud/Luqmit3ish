using Luqmit3ish.Models;
using Luqmit3ish.Services;
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
        public ICommand LoginCommand { get; }

        public UserServices userServices;
        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            ForgotPassCommand = new Command(() => OnForgotPassClicked());
            SignupCommand = new Command(() => OnSignupClicked());
            LoginCommand = new Command(() => OnLoginClicked());
            userServices = new UserServices();
        }
        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        private async void OnForgotPassClicked()
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }
        private async void OnSignupClicked()
        {
            await Navigation.PushModalAsync(new SignupPage());
        }

        private async void OnLoginClicked()
        {
            LoginRequest loginRequest = new LoginRequest()
            {
                Email = email,
                Password = password
            };
            await canLogin(loginRequest);
        }
        public async Task<bool> canLogin(LoginRequest loginRequest)
        {
            bool hasAccount = await userServices.Login(loginRequest);
            if (hasAccount)
            {
                Application.Current.MainPage = new AppShell();
                return true;
            }
            await Application.Current.MainPage.DisplayAlert("Invalid input", "You have entered an invalid username or password", "Ok");
            return false;
        }
        private bool loginButtonEnable = false;
        public bool LoginButtonEnable
        {
            get => loginButtonEnable;
            set
            {
                if (loginButtonEnable == value) return;
                loginButtonEnable = value;
                OnPropertyChanged(nameof(LoginButtonEnable));
            }
        }
        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (email == value) return;
                email = value;
                OnPropertyChanged(nameof(Email));
                if (email != null)
                    LoginButtonEnable = true;
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (password == value) return;
                password = value;
                OnPropertyChanged(nameof(Password));
                if (password != null)
                    LoginButtonEnable = true;
            }
        }
    }
}

