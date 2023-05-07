using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using Luqmit3ish.Exceptions;
using System.Net.Http;
using Rg.Plugins.Popup.Services;
using System.Threading;
using System.Collections.Generic;

namespace Luqmit3ish.ViewModels
{
    class SignupViewModel : ViewModelBase
    {
        public INavigation _navigation { get; set; }

        public ICommand signupClicked { protected set; get; }
        public ICommand LoginCommand { protected set; get; }
        public ICommand HidePasswordCommand { protected set; get; }
        public ICommand ShowPasswordCommand { protected set; get; }
        private readonly UserServices userServices;

      
        #region Password
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
        private SignUpRequest _userInfo;
        public SignUpRequest UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }

        private bool _hidePassword = true;
        public bool HidePassword
        {
            get => _hidePassword;
            set => SetProperty(ref _hidePassword, value);

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

        public SignupViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            signupClicked = new Command(async () => await OnSignupClicked());
            LoginCommand = new Command(async () => await OnLoginClicked());
            ShowPasswordCommand = new Command(OnUnHidePasswordClicked);
            HidePasswordCommand = new Command(OnHidePasswordClicked);
            userServices = new UserServices();
            UserInfo = new SignUpRequest();
        }


        #region EmailValidation
        private bool _isEmailValid;
        public bool IsEmailValid
        {
            get => _isEmailValid;
            set
            {

                SetProperty(ref _isEmailValid, value);
                if (IsEmailValid)
                {
                    _emailInValid = false;
                    _emailFrameBorder = "Transparent";
                }
                else
                {
                    _emailInValid = true;
                    _emailFrameBorder = "Red";
                }
                OnPropertyChanged(nameof(EmailInValid));
                OnPropertyChanged(nameof(EmailFrameBorder));
            }
        }
        private bool _emailInValid = false;
        public bool EmailInValid
        {
            get => _emailInValid;
            set
            {
                SetProperty(ref _emailInValid, value);

            }
        }
        private string _emailErrorMessage = "Please enter a valid email";
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set
            {
                SetProperty(ref _emailErrorMessage, value);
            }
        }
        private string _emailFrameBorder = "Transparent";
        public string EmailFrameBorder
        {
            get => _emailFrameBorder;
            set
            {
                SetProperty(ref _emailFrameBorder, value);
            }
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
                    _passwordInvalid = false;
                    _passwordFrameBorder = "Transparent";
                }
                else
                {
                    _passwordInvalid = true;
                    _passwordFrameBorder = "Red";
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
        private string _passwordErrorMessage = "Enter at least 8 characters including one number, one uppercase letter and a special character.";
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set
            {
                SetProperty(ref _passwordErrorMessage, value);
            }

        }

        private string _passwordFrameBorder = "Transparent";
        public string PasswordFrameBorder
        {
            get => _passwordFrameBorder;
            set
            {
                SetProperty(ref _passwordFrameBorder, value);
            }
        }

        #endregion

        #region PasswordValidation
        private bool _isPhoneValid;
        public bool IsPhoneValid
        {
            get => _isPhoneValid;
            set
            {

                SetProperty(ref _isPhoneValid, value);
                if (IsPhoneValid)
                {
                    _phoneInvalid = false;
                    _phoneFrameBorder = "Transparent";
                }
                else
                {
                    _phoneInvalid = true;
                    _phoneFrameBorder = "Red";
                }
                OnPropertyChanged(nameof(PhoneInvalid));
                OnPropertyChanged(nameof(PhoneFrameBorder));
            }
        }
        private bool _phoneInvalid = false;
        public bool PhoneInvalid
        {
            get => _phoneInvalid;
            set
            {
                SetProperty(ref _phoneInvalid, value);

            }
        }
        private string _phoneErrorMessage = "Please enter a valid phone number.";
        public string PhoneErrorMessage
        {
            get => _phoneErrorMessage;
            set
            {
                SetProperty(ref _phoneErrorMessage, value);
            }

        }

        private string _phoneFrameBorder = "Transparent";
        public string PhoneFrameBorder
        {
            get => _phoneFrameBorder;
            set
            {
                SetProperty(ref _phoneFrameBorder, value);
            }
        }

        #endregion

        private bool CheckEmptyFields()
        {
            if (string.IsNullOrEmpty(_userInfo.Email) || string.IsNullOrEmpty(_userInfo.Type) || string.IsNullOrEmpty(_userInfo.Name)
                    || string.IsNullOrEmpty(_userInfo.Password) || string.IsNullOrEmpty(_userInfo.Phone))
            {
                return true;
            }
            return false;
        }

        private bool ValidateFields()
        {
            if (_emailInValid || _passwordInvalid || _phoneInvalid)
            {
                return true;
            }
            return false;
        }
        private bool CheckEmailExistent(ObservableCollection<User> users)
        {
                if (!Object.ReferenceEquals(users, null))
                {
                    foreach (User user in users)
                    {
                        if (user.Email == _userInfo.Email)
                        {
                            return true;
                        }
                    }
                }
                return false;

        }

        private async Task OnSignupClicked()
        {
            try
            {

                if (CheckEmptyFields())
                {
                    await PopupNavigation.Instance.PushAsync(new PopUp("Please fill all the fields."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }
                if (ValidateFields())
                {
                    await PopupNavigation.Instance.PushAsync(new PopUp("Please enter valid fields before continue."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }

                ObservableCollection<User> users = await GetUsers();

                if (CheckEmailExistent(users))
                {
                    await PopupNavigation.Instance.PushAsync(new PopUp("The email is already exists."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }
                
                    Application.Current.MainPage = new VerificationPage(_userInfo);
                
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async Task OnLoginClicked()
        {
            try
            {
                await _navigation.PushModalAsync(new LoginPage());

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

        private async Task<ObservableCollection<User>> GetUsers()
        {
            
                try
                {
                    return await userServices.GetUsers();
                }
                catch (ConnectionException e)
                {
                Debug.WriteLine(e.Message);
                return null;
                   
                }
                catch (HttpRequestException e)
                {
                Debug.WriteLine(e.Message);
                return null;
                }
                catch (Exception e)
                {
                Debug.WriteLine(e.Message);
                return null;
            }

            
        }
    
    }
}
