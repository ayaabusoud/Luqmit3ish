using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class ResetPassSettingsViewModel: ViewModelBase
    {
        private INavigation Navigation { get; set; }

        public ICommand ResetPasswordCommand { protected set; get; }
        public ICommand HidePasswordCommand { protected set; get; }
        public ICommand UnHidePasswordCommand { protected set; get; }


        public readonly IUserServices _userServices;


        public ResetPassSettingsViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            ResetPasswordCommand = new Command(async () => await OnResetClicked());
            UnHidePasswordCommand = new Command(OnUnHidePasswordClicked);
            HidePasswordCommand = new Command(OnHidePasswordClicked);
            _userServices = new UserServices();
        }
      

        private void OnUnHidePasswordClicked()
        {
            _passwordHidden = false;
            _passwordUnHidden = true;
            _isPassword = false;
            OnPropertyChanged(nameof(IsPassword));
            OnPropertyChanged(nameof(PasswordHidden));
            OnPropertyChanged(nameof(PasswordUnHidden));

        }

        private void OnHidePasswordClicked()
        {
            _passwordHidden = true;
            _passwordUnHidden = false;
            _isPassword = true;
            OnPropertyChanged(nameof(IsPassword));
            OnPropertyChanged(nameof(PasswordHidden));
            OnPropertyChanged(nameof(PasswordUnHidden));
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);

                if (IsValidPassword(_password))
                {
                    _passwordErrorVisible = false;

                }
                else
                {
                    _passwordErrorVisible = true;


                }
                OnPropertyChanged(nameof(PasswordErrorVisible));

            }
        }

        private string _passwordErrorMessage = "Your password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.";
        public string PasswordErrorMessage
        {
            get { return _passwordErrorMessage; }
            set
            {
                SetProperty(ref _passwordErrorMessage, value);

            }
        }

        private bool _passwordHidden = true;
        public bool PasswordHidden
        {
            get => _passwordHidden;
            set
            {
                SetProperty(ref _passwordHidden, value);
                OnPropertyChanged(nameof(PasswordHidden));
            }
        }
        private bool _isPassword = true;
        public bool IsPassword
        {
            get => _isPassword;
            set
            {
                SetProperty(ref _isPassword, value);

                OnPropertyChanged(nameof(IsPassword));
            }
        }
        private bool _passwordErrorVisible;
        public bool PasswordErrorVisible
        {
            get => _passwordErrorVisible;
            set
            {
                SetProperty(ref _passwordErrorVisible, value);
            }
        }



        private bool IsValidPassword(string password)
        {
            string passwordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            if (string.IsNullOrEmpty(password)) return false;
            if (Regex.IsMatch(password, passwordPattern))
            {
                return true;
            }
            return false;
        }
        private bool _passwordUnHidden;
        public bool PasswordUnHidden
        {
            get => _passwordUnHidden;
            set
            {
                SetProperty(ref _passwordUnHidden, value);
            }
        }






        private async Task OnResetClicked()
        {
            try
            {

                var id = Preferences.Get("userId", null);
                if (id == null)
                {
                    return;
                }
                var userId = int.Parse(id);
                if (!IsValidPassword(Password))
                {
                    return;
                }
                bool IsUpdatedPassword = await _userServices.ResetPassword(userId, Password);
                if (IsUpdatedPassword)
                {
                    await Navigation.PushModalAsync(new LoginPage());
                    await App.Current.MainPage.DisplayAlert("Done", "Your password has been successfully reset.", "OK");

                }

            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(HttpRequestMessage);
            }
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                NotAuthorized();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }
    }
}
