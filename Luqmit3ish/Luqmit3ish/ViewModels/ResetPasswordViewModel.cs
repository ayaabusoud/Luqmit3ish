using Luqmit3ish.Exceptions;
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
    class ResetPasswordViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand ResetPasswordCommand { protected set; get; }
        public ICommand HidePasswordCommand { protected set; get; }
        public ICommand UnHidePasswordCommand { protected set; get; }
        public ICommand HideNewPasswordCommand { protected set; get; }
        public ICommand UnHideNewPasswordCommand { protected set; get; }


        public readonly UserServices userServices;
        private string _email;


        public ResetPasswordViewModel(INavigation navigation,string email)
        {
            this._navigation = navigation;
            ResetPasswordCommand = new Command(async () => await OnResetClicked());
            UnHidePasswordCommand = new Command(OnUnHidePasswordClicked);
            HidePasswordCommand = new Command(OnHidePasswordClicked);

            UnHideNewPasswordCommand = new Command(OnUnHideNewPasswordClicked);
            HideNewPasswordCommand = new Command(OnHideNewPasswordClicked);
            userServices = new UserServices();
            _email = email;
            
        }

        private void OnUnHidePasswordClicked()
        {
            _passwordHidden = false;
            _passwordUnHidden = true;
            _isPassword = false;
            OnPropertyChanged(nameof(PasswordHidden));
            OnPropertyChanged(nameof(PasswordUnHidden));
            OnPropertyChanged(nameof(IsPassword));
        }

        private void OnHidePasswordClicked()
        {
            _passwordHidden = true;
            _passwordUnHidden = false;
            _isPassword = true;
            OnPropertyChanged(nameof(PasswordHidden));
            OnPropertyChanged(nameof(PasswordUnHidden));
            OnPropertyChanged(nameof(IsPassword));
        }

        private void OnUnHideNewPasswordClicked()
        {
            _newPasswordHidden = false;
            _newPasswordUnHidden = true;
            _isPasswordNew = false;
            OnPropertyChanged(nameof(NewPasswordHidden));
            OnPropertyChanged(nameof(NewPasswordUnHidden));
            OnPropertyChanged(nameof(IsPasswordNew));
        }

        private void OnHideNewPasswordClicked()
        {
            _newPasswordHidden = true;
            _newPasswordUnHidden = false;
            _isPasswordNew = true;
            OnPropertyChanged(nameof(NewPasswordHidden));
            OnPropertyChanged(nameof(NewPasswordUnHidden));
            OnPropertyChanged(nameof(IsPasswordNew));
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set {
                SetProperty(ref _oldPassword, value);
                if(string.IsNullOrEmpty(_oldPassword))
                {
                    _passwordHidden = false;
                    OnPropertyChanged(nameof(PasswordHidden));
                }
                else
                {
                    _passwordHidden = true;
                    OnPropertyChanged(nameof(PasswordHidden));
                }
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                SetProperty(ref _newPassword, value);
                if (string.IsNullOrEmpty(_newPassword))
                {
                    _newPasswordHidden = false;
                    OnPropertyChanged(nameof(NewPasswordHidden));

                }
                else
                {
                    _newPasswordHidden = true;
                    OnPropertyChanged(nameof(NewPasswordHidden));
                }
            }
        }

        private Color _passwordFrameColor;
        public Color PasswordFrameColor
        {
            get => _passwordFrameColor;
            set => SetProperty(ref _passwordFrameColor, value);
        }

        private bool _passwordUnHidden = false;
        public bool PasswordUnHidden
        {
            get => _passwordUnHidden;
            set => SetProperty(ref _passwordUnHidden, value);

        }

        private bool _passwordHidden = false;
        public bool PasswordHidden
        {
            get => _passwordHidden;
            set => SetProperty(ref _passwordHidden, value);

        }

        private bool _newPasswordHidden = false;
        public bool NewPasswordHidden
        {
            get => _newPasswordHidden;
            set => SetProperty(ref _newPasswordHidden, value);
        }

        private bool _newPasswordUnHidden = false;
        public bool NewPasswordUnHidden
        {
            get => _newPasswordUnHidden;
            set => SetProperty(ref _newPasswordUnHidden, value);
        }

        private bool _isPassword= true;
        public bool IsPassword
        {
            get => _isPassword;
            set => SetProperty(ref _isPassword, value);
        }

        private bool _isPasswordNew = true;
        public bool IsPasswordNew
        {
            get => _isPasswordNew;
            set => SetProperty(ref _isPasswordNew, value);
        }

        private bool _passwordErrorVisible;
        public bool PasswordErrorVisible
        {
            get => _passwordErrorVisible;
            set=> SetProperty(ref _passwordErrorVisible, value);

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
        private string _messageError;
        public string MessageError
        {
            get => _messageError;
            set => SetProperty(ref _messageError, value);
        }

        private async Task OnResetClicked()
        {
            try
            {
                Debug.WriteLine(_email);
                User user = await userServices.GetUserByEmail(Email);
                if (user == null)
                {
                    return;
                }

                byte[] passwordBytes = Encoding.UTF8.GetBytes(_oldPassword);
                string encodedPassword = Convert.ToBase64String(passwordBytes);

                if (!encodedPassword.Equals(user.Password))
                {
                    _messageError = "Old password is incorrect. Please try again.";
                    _passwordErrorVisible = true;
                    OnPropertyChanged(nameof(PasswordErrorVisible));
                    OnPropertyChanged(nameof(MessageError));
                    return;
                }

                if (OldPassword.Equals(NewPassword))
                {
                    _messageError = "New password cannot be the same as the old password.";
                    _passwordErrorVisible = true;
                    OnPropertyChanged(nameof(PasswordErrorVisible));
                    OnPropertyChanged(nameof(MessageError));
                    return;
                }

                if (!IsValidPassword(NewPassword))
                {
                    _messageError = "Enter at least 8 characters including one number, one uppercase letter and a special character.";
                    _passwordErrorVisible = true;
                    OnPropertyChanged(nameof(PasswordErrorVisible));
                    OnPropertyChanged(nameof(MessageError));
                    return;
                }

                bool IsUpdatedPassword = await userServices.ResetPassword(user.Id, NewPassword);
                if (IsUpdatedPassword)
                {
                    _passwordErrorVisible = false;
                    OnPropertyChanged(nameof(PasswordErrorVisible));
                    await PopupNavigation.Instance.PushAsync(new PopUp("Your password has been successfully reset."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (ConnectionException )
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was a connection error. Please check your internet connection and try again.", "OK");
            }
            catch (HttpRequestException )
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was an HTTP request error. Please try again later.", "OK");

            }
            catch (Exception )
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred. Please try again later.", "OK");
            }
        }
    }
}
