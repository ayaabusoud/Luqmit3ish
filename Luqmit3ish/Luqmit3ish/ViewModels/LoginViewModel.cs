using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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
            try
            {
            await Navigation.PushAsync(new ForgotPasswordPage());

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
        private async void OnSignupClicked()
        {
            try
            {
                await Navigation.PushModalAsync(new SignupPage());
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
            try
            {
                bool hasAccount = await userServices.Login(loginRequest);
                if (hasAccount)
                {
                    Preferences.Set("userEmail", Email);
                    Application.Current.MainPage = new AppShellCharity();
                    return true;
                }
                await Application.Current.MainPage.DisplayAlert("Invalid input", "You have entered an invalid username or password", "Ok");
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                return false;
                Debug.WriteLine(e.Message);
            }
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

