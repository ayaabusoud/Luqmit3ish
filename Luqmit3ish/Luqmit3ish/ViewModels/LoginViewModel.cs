using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        public ICommand HidePasswordCommand { protected set; get; }
        public ICommand ShowPasswordCommand { protected set; get; }
        private UserServices _userServices;

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ForgotPassCommand = new Command(OnForgotPassClicked);
            SignupCommand = new Command(() => OnSignupClicked());
            LoginCommand = new Command(() => OnLoginClicked());
            ShowPasswordCommand = new Command(OnUnHidePasswordClicked);
            HidePasswordCommand = new Command(OnHidePasswordClicked);
            _userServices = new UserServices();
        }

        private bool _loginButtonEnable = false;
        public bool LoginButtonEnable
        {
            get => _loginButtonEnable;
            set => SetProperty(ref _loginButtonEnable, value);
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

                    string token = Preferences.Get("Token", string.Empty);
                    string userId = string.Empty;
                    string userEmail = string.Empty;
                    string userType = string.Empty;
                    if (!string.IsNullOrEmpty(token))
                    {
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

                        userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                        userEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                        userType = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                    }


                    Preferences.Set("userEmail", userEmail);
                    Preferences.Set("userId", userId);
                    if (userType.Equals("Restaurant"))
                    {
                        Application.Current.MainPage = new AppShellRestaurant();

                    }
                    else
                    {
                        Application.Current.MainPage = new AppShellCharity();
                    }
                    return true;
                }
                await PopNavigationAsync("You have entered an invalid username or password.");
                return false;
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
                return false;
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
                return false;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(HttpRequestMessage);
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
                return false;
            }
        }

        private void OnForgotPassClicked()
        {
            try
            {
                Application.Current.MainPage = new NavigationPage(new ForgotPasswordPage()) ;

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

        #region InputFields
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
                if (_hidePassword)
                {
                    _hidePassword = true;
                    _showPassword = false;
                    _isPassword = true;
                }
                else
                {
                    _hidePassword = false;
                    _showPassword = true;
                    _isPassword = false;
                }
                if (_password != null)
                {
                    LoginButtonEnable = true;

                }
                OnPropertyChanged(nameof(HidePassword));
                OnPropertyChanged(nameof(ShowPassword));
                OnPropertyChanged(nameof(IsPassword));

            }
        }
        #endregion
        #region PasswordFeatures

        private bool _isPassword = true;
        public bool IsPassword
        {
            get => _isPassword;
            set => SetProperty(ref _isPassword, value);

        }

        private bool _showPassword = false;
        public bool ShowPassword
        {
            get => _showPassword;
            set => SetProperty(ref _showPassword, value);

        }
        private bool _hidePassword = true;
        public bool HidePassword
        {
            get => _hidePassword;
            set => SetProperty(ref _hidePassword, value);

        }
        private void OnHidePasswordClicked()
        {
            IsPassword = false;
            ShowPassword = true;
            HidePassword = false;
        }
        private void OnUnHidePasswordClicked()
        {
            IsPassword = true;
            ShowPassword = false;
            HidePassword = true;
        }
        #endregion

    }
}
