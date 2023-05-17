using Luqmit3ish.Exceptions;
using Luqmit3ish.Hashing;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Utilities;
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
        public ICommand ShowPasswordCommand { protected set; get; }
        public ICommand HideOldPasswordCommand { protected set; get; }
        public ICommand ShowOldPasswordCommand { protected set; get; }

        private readonly IHasher _hashing;
        private readonly UserServices _userServices;
        private string _email;
        private const string _samePasswordMessage = "New password cannot be the same as the old password.";
        private const string _incorrectPassword = "Old password is incorrect. Please try again.";
        private const string _passwordResetSuccessMessage = "Your password has been successfully reset.";



        public ResetPasswordViewModel(INavigation navigation, string email)
        {
            this._navigation = navigation;
            ResetPasswordCommand = new Command(async () => await OnResetClicked());
            ShowPasswordCommand = new Command(OnUnHidePasswordClicked);
            HidePasswordCommand = new Command(OnHidePasswordClicked);

            UnHideNewPasswordCommand = new Command(OnUnHideNewPasswordClicked);
            HideNewPasswordCommand = new Command(OnHideNewPasswordClicked);
            _userServices = new UserServices();
            _email = email;
            _hashing = new PasswordHasher();

            ShowOldPasswordCommand = new Command(OnUnHideOldPasswordClicked);
            HideOldPasswordCommand = new Command(OnHideOldPasswordClicked);
            userServices = new UserServices();
            _email = email;

        }
        private SignUpRequest _userInfo;
        public SignUpRequest UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }
        #region Old Password
        private bool _isOldPassword = true;
        public bool IsOldPassword
        {
            get => _isOldPassword;
            set => SetProperty(ref _isOldPassword, value);

        }

        private bool _showOldPassword = false;
        public bool ShowOldPassword
        {
            get => _showOldPassword;
            set => SetProperty(ref _showOldPassword, value);

        }

        private bool _hideOldPassword = true;
        public bool HideOldPassword
        {
            get => _hideOldPassword;
            set => SetProperty(ref _hideOldPassword, value);

        }

        #endregion

        #region New Password
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

        #endregion
        #region PasswordValidation
        private bool _isPasswordValid;

        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set
            {
                SetProperty(ref _isPasswordValid, value);

                if (IsPasswordValid)
                {
                    PasswordInvalid = false;
                    PasswordFrameBorder = PasswordFrameBorderStyle.Transparent;
                }
                else
                {
                    PasswordInvalid = true;
                    PasswordFrameBorder = PasswordFrameBorderStyle.Red;
                }

                OnPropertyChanged(nameof(PasswordInvalid));
                OnPropertyChanged(nameof(PasswordFrameBorder));
            }
        }

        private bool _passwordInvalid = false;
        public bool PasswordInvalid
        {
            get => _passwordInvalid;
            set
            {
                SetProperty(ref _passwordInvalid, value);
            }
        }

        private string _passwordErrorMessage = PasswordErrorMessages.PasswordRequirements;
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set
            {
                SetProperty(ref _passwordErrorMessage, value);
            }
        }

        private string _passwordFrameBorder = PasswordFrameBorderStyle.Transparent;
        public string PasswordFrameBorder
        {
            get => _passwordFrameBorder;
            set
            {
                SetProperty(ref _passwordFrameBorder, value);
            }
        }

        #endregion



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

        private void OnHideOldPasswordClicked()
        {
            IsOldPassword = false;
            ShowOldPassword = true;
            HideOldPassword = false;
        }
        private void OnUnHideOldPasswordClicked()
        {
            IsOldPassword = true;
            ShowOldPassword = false;
            HideOldPassword = true;

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
            set
            {
                SetProperty(ref _oldPassword, value);
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                SetProperty(ref _newPassword, value);
            }
        }



        private async Task OnResetClicked()
        {
            try
            {
                Debug.WriteLine(_email);
                User user = await _userServices.GetUserByEmail(Email);
                if (user == null)
                {
                    return;
                }
                if (!_hashing.VerifyPassword(_oldPassword, user.Password))
                {
                    _messageError = "Old password is incorrect. Please try again.";
                    _passwordErrorVisible = true;
                    OnPropertyChanged(nameof(PasswordErrorVisible));
                    OnPropertyChanged(nameof(MessageError));
                    return;
                }


                if (await ValidateFields(user))
                {
                    bool IsUpdatedPassword = await _userServices.ResetPassword(user.Id, NewPassword);
                    if (IsUpdatedPassword)
                    {

                        await _navigation.PopAsync();
                        await PopNavigationAsync(_passwordResetSuccessMessage);
                    }
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
                await PopNavigationAsync(NotAuthorizedMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task<bool> ValidateFields(User user)
        {
            if (string.IsNullOrEmpty(_oldPassword) || string.IsNullOrEmpty(_newPassword))
            {
                await PopNavigationAsync(PasswordErrorMessages.EmptyField);
                return false;
            }

            if (!PasswordHasher.VerifyPassword(_oldPassword, user.Password))
            {
                await PopNavigationAsync(_incorrectPassword);
                return false;
            }

            if (OldPassword.Equals(NewPassword))
            {
                await PopNavigationAsync(_samePasswordMessage);
                return false;
            }         

            if (!_isPasswordValid)
            {
                await PopNavigationAsync(PasswordErrorMessages.InvalidPassword);
                return false;
            }

            return true;
        }

    }
}
