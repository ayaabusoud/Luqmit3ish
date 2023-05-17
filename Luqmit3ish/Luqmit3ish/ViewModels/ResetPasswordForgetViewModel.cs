using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Utilities;
using Luqmit3ish.Views;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    class ResetPasswordForgetViewModel : ViewModelBase
    {
        #region Properties

        private INavigation _navigation { get; set; }

        public ICommand ResetPasswordCommand { protected set; get; }
        public ICommand HidePasswordCommand { protected set; get; }
        public ICommand UnHidePasswordCommand { protected set; get; }

        public readonly IUserServices _userServices;

        private string _email;
        private const string _passwordResetSuccessMessage = "Your password has been successfully reset.";

        #endregion

        #region Constructor

        public ResetPasswordForgetViewModel(INavigation navigation, string email)
        {
            this._navigation = navigation;
            ResetPasswordCommand = new Command(async () => await OnResetClicked());
            UnHidePasswordCommand = new Command(OnUnHidePasswordClicked);
            HidePasswordCommand = new Command(OnHidePasswordClicked);
            _userServices = new UserServices();
            _email = email;
        }
        #endregion


        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                OnPropertyChanged(nameof(Email));
            }
        }

        #region PasswordFieldFeatures
        private void OnUnHidePasswordClicked()
        {
            _passwordHidden = false;
            _passwordUnHidden = true;
            _isPassword = false;
            OnPropertyChanged(nameof(IsPassword));
            OnPropertyChanged(nameof(PasswordHidden));
            OnPropertyChanged(nameof(PasswordUnHidden));


        #region Password Properties

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

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        #region Password Validation Properties

        private bool _isPasswordValid;
        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set
            {
                SetProperty(ref _isPasswordValid, value);
                if (IsPasswordValid)
                {
                    _passwordInvalid = false;
                    _passwordFrameBorder = PasswordFrameBorderStyle.Transparent;
                }
                else
                {
                    _passwordInvalid = true;
                    _passwordFrameBorder = PasswordFrameBorderStyle.Red;
                }
                OnPropertyChanged(nameof(PasswordInvalid));
                OnPropertyChanged(nameof(PasswordFrameBorder));
            }
        }

        private bool _passwordInvalid = false;
        public bool PasswordInvalid
        {
            get => _passwordInvalid;
            set => SetProperty(ref _passwordInvalid, value);
        }

        private string _passwordErrorMessage = PasswordErrorMessages.PasswordRequirements;
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetProperty(ref _passwordErrorMessage, value);
        }

        private string _passwordFrameBorder = PasswordFrameBorderStyle.Transparent;
        public string PasswordFrameBorder
        {
            get => _passwordFrameBorder;
            set => SetProperty(ref _passwordFrameBorder, value);
        }

        #endregion

        #region Methods

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
        private async Task OnResetClicked()
        {
            try
            {
                User user = await _userServices.GetUserByEmail(Email);

                if (user == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(_password))
                {
                    await PopNavigationAsync(PasswordErrorMessages.EmptyField);
                    return;
                }

                if (!_isPasswordValid)
                {
                    await PopNavigationAsync(PasswordErrorMessages.InvalidPassword);
                    return;
                }

                bool IsUpdatedPassword = await _userServices.ForgotPassword(user.Id, _password);
                if (IsUpdatedPassword)
                {
                    await _navigation.PushModalAsync(new LoginPage());
                    await PopupNavigation.Instance.PushAsync(new PopUp(_passwordResetSuccessMessage));
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
                await PopNavigationAsync(NotAuthorizedMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        #endregion

        #region Additional Properties

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                OnPropertyChanged(nameof(Email));
            }
        }

        #endregion
    }
}
