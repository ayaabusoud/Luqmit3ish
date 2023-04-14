using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand ForgotPassCommand { get; }
        public ICommand SignupCommand { get; }
        public ICommand LoginCommand { get; }

        private UserServices _userServices;
        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ForgotPassCommand = new Command(() => OnForgotPassClicked());
            SignupCommand = new Command(() => OnSignupClicked());
            LoginCommand = new Command(() => OnLoginClicked());
            _userServices = new UserServices();
        }

        private async void OnForgotPassClicked()
        {
            try
            {
                await _navigation.PushAsync(new ForgotPasswordPage());

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
                await _navigation.PushModalAsync(new SignupPage());
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
                Email = _email,
                Password = _password
            };
            await CanLogin(loginRequest);
        }
        public async Task<bool> CanLogin(LoginRequest loginRequest)
        {
            try
            {
                bool hasAccount = await _userServices.Login(loginRequest);
                if (hasAccount)
                {
                    User user = await _userServices.GetUserByEmail(Email);

                    Preferences.Set("userEmail", Email);
                    Preferences.Set("userId", user.id.ToString());
                    if (user.Type.Equals("Restaurant"))
                    {
                        Application.Current.MainPage = new AppShellRestaurant();
                    }
                    else
                    {
                        Application.Current.MainPage = new AppShellCharity();
                    }
                    return true;
                }

                await Application.Current.MainPage.DisplayAlert("Invalid input", "You have entered an invalid username or password", "Ok");
                return false;
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            catch (HttpRequestException)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid credentials", "Please check your username and password and try again", "Ok");
                return false;
            }
            catch (ConnectionException)
            {
                await Application.Current.MainPage.DisplayAlert("Bad request", $"There is no internet connection, please check your connection", "Ok");
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        private bool _loginButtonEnable = false;
        public bool LoginButtonEnable
        {
            get => _loginButtonEnable;
            set => SetProperty(ref _loginButtonEnable, value);
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                if (_email != null)
                    LoginButtonEnable = true;
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                if (_password != null)
                    LoginButtonEnable = true;
            }
        }
    }
}
