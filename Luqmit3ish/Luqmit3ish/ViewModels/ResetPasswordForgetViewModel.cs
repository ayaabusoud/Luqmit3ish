using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    class ResetPasswordForgetViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand ResetPasswordCommand { protected set; get; }
        public ICommand HidePasswordCommand { protected set; get; }
        public ICommand UnHidePasswordCommand { protected set; get; }


        public readonly IUserServices _userServices;
        private string _email;

        public ResetPasswordForgetViewModel(INavigation navigation, string email)
        {
            this._navigation = navigation;
            ResetPasswordCommand = new Command(async () => await OnResetClicked());
            UnHidePasswordCommand = new Command(OnUnHidePasswordClicked);
            HidePasswordCommand = new Command(OnHidePasswordClicked);
            _userServices = new UserServices();
            _email = email;

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
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                if (string.IsNullOrEmpty(_password))
                {
                    _passwordHidden = false;
                    OnPropertyChanged(nameof(PasswordHidden));
                }
                else
                {
                    _passwordHidden = true ;
                    OnPropertyChanged(nameof(PasswordHidden));

                }
                if (IsValidPassword(_password))
                {
                    _passwordErrorVisible = false;
                    OnPropertyChanged(nameof(PasswordErrorVisible));
                }
                else
                {
                    _passwordErrorVisible = true;
                }
                OnPropertyChanged(nameof(PasswordErrorVisible));
            }
        }

        private string _passwordErrorMessage = "Enter at least 8 characters including one number, one uppercase letter and a special character.";
        public string PasswordErrorMessage
        {
            get { return _passwordErrorMessage; }
            set
            {
                SetProperty(ref _passwordErrorMessage, value);
                OnPropertyChanged(nameof(PasswordErrorMessage));
            }
        }

        private bool _passwordHidden = false;
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
                OnPropertyChanged(nameof(PasswordErrorVisible));

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
                if (!IsValidPassword(Password))
                {
                    return;
                }
                Debug.WriteLine(_email);
                User user = await _userServices.GetUserByEmail(Email);

                if (user == null)
                {
                    return;
                }


                bool IsUpdatedPassword = await _userServices.ResetPassword(user.Id, Password);
                if (IsUpdatedPassword)
                {
                    await _navigation.PushModalAsync(new LoginPage());
                    await PopupNavigation.Instance.PushAsync(new PopUp("Your password has been successfully reset."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
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
