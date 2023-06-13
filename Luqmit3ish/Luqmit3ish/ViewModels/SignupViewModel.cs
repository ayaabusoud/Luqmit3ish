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

        private const string FillFieldsMessage = "Please fill all the fields.";
        private const string ValidFieldsMessage = "Please enter valid fields before continue.";
        private const string EmailExistsMessage = "The email is already exists.";

        private const string ValidFrameBorderColor = "Transparent";
        private const string InValidFrameBorderColor = "DarkRed";

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

        private void UpdateFieldValidity(ref bool isFieldValid, ref bool fieldInvalid, ref string fieldFrameBorder, params string[] propertyNames)
        {
            if (isFieldValid)
            {
                fieldInvalid = false;
                fieldFrameBorder = ValidFrameBorderColor;
            }
            else
            {
                fieldInvalid = true;
                fieldFrameBorder = InValidFrameBorderColor;
            }

            foreach (var propertyName in propertyNames)
            {
                OnPropertyChanged(propertyName);
            }
        }

        #region Name Validation
        private bool _isNameValid;
        public bool IsNameValid
        {
            get => _isNameValid;
            set
            {
                SetProperty(ref _isNameValid, value);
                UpdateFieldValidity(ref _isNameValid, ref _nameInvalid, ref _nameFrameBorder, nameof(IsNameValid), nameof(NameInvalid), nameof(NameFrameBorder));
            }
        }
        private bool _nameInvalid = false;
        public bool NameInvalid
        {
            get => _nameInvalid;
            set
            {
                SetProperty(ref _nameInvalid, value);
            }
        }
        private string _nameErrorMessage = "Please enter a valid name without symbols.";
        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set
            {
                SetProperty(ref _nameErrorMessage, value);
            }
        }

        private string _nameFrameBorder = ValidFrameBorderColor;
        public string NameFrameBorder
        {
            get => _nameFrameBorder;
            set => SetProperty(ref _nameFrameBorder, value);
        }

        #endregion

        #region EmailValidation
        private bool _isEmailValid;
        public bool IsEmailValid
        {
            get => _isEmailValid;
            set
            {
                SetProperty(ref _isEmailValid, value);
                UpdateFieldValidity(ref _isEmailValid, ref _emailInValid, ref _emailFrameBorder, nameof(IsEmailValid), nameof(_emailFrameBorder), nameof(EmailFrameBorder),nameof(EmailInValid));
            }
        }
        private bool _emailInValid = false;
        public bool EmailInValid
        {
            get => _emailInValid;
            set => SetProperty(ref _emailInValid, value);
        }
        private string _emailErrorMessage = "Please enter a valid email";
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set => SetProperty(ref _emailErrorMessage, value);
            
        }
        private string _emailFrameBorder = "Transparent";
        public string EmailFrameBorder
        {
            get => _emailFrameBorder;
            set => SetProperty(ref _emailFrameBorder, value);
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
                UpdateFieldValidity(ref _isPasswordValid,ref _passwordInvalid,ref _passwordFrameBorder, nameof(PasswordInvalid), nameof(PasswordFrameBorder));  
            }
        }
        private bool _passwordInvalid = false;
        public bool PasswordInvalid
        {
            get => _passwordInvalid;
            set => SetProperty(ref _passwordInvalid, value);
        }
        private string _passwordErrorMessage = "Enter at least 8 characters including one number, one uppercase letter and a special character.";
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetProperty(ref _passwordErrorMessage, value);
        }

        private string _passwordFrameBorder = ValidFrameBorderColor;
        public string PasswordFrameBorder
        {
            get => _passwordFrameBorder;
            set => SetProperty(ref _passwordFrameBorder, value);
        }

        #endregion

        #region PhoneValidation
        private bool _isPhoneValid;
        public bool IsPhoneValid
        {
            get => _isPhoneValid;
            set
            {
                SetProperty(ref _isPhoneValid, value);
                UpdateFieldValidity(ref _isPhoneValid, ref _phoneInvalid, ref _phoneFrameBorder, nameof(PhoneInvalid), nameof(PhoneFrameBorder));
            }
        }
        private bool _phoneInvalid = false;
        public bool PhoneInvalid
        {
            get => _phoneInvalid;
            set => SetProperty(ref _phoneInvalid, value);
        }
        private string _phoneErrorMessage = "Please enter a valid phone number.";
        public string PhoneErrorMessage
        {
            get => _phoneErrorMessage;
            set => SetProperty(ref _phoneErrorMessage, value);
        }

        private string _phoneFrameBorder = ValidFrameBorderColor;
        public string PhoneFrameBorder
        {
            get => _phoneFrameBorder;
            set => SetProperty(ref _phoneFrameBorder, value);
        }
        #endregion

        private bool CheckEmptyFields()
        {
            return (
                string.IsNullOrEmpty(_userInfo.Email) ||
                string.IsNullOrEmpty(_userInfo.Type) ||
                string.IsNullOrEmpty(_userInfo.Name) ||
                string.IsNullOrEmpty(_userInfo.Password) ||
                string.IsNullOrEmpty(_userInfo.Phone)
            );
        }

        private bool ValidateFields()
        {
            return (_emailInValid || _passwordInvalid || _phoneInvalid) ;
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
                    await PopNavigationAsync(FillFieldsMessage);
                    return;
                }
                if (ValidateFields())
                {
                    await PopNavigationAsync(ValidFieldsMessage);
                    return;
                }

                ObservableCollection<User> users = await GetUsers();

                if (CheckEmailExistent(users))
                {
                    await PopNavigationAsync(EmailExistsMessage);
                    return;
                }
                Application.Current.MainPage = new VerificationPage(_userInfo);
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
            catch (EmptyIdException e)
            {
                Debug.WriteLine(e.Message);
                EndSession();
            }
            catch (EmailNotFoundException e)
            {
                Debug.WriteLine(e.Message);
                EndSession();
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

        private async Task OnLoginClicked()
        {
            try
            {
                await _navigation.PushModalAsync(new LoginPage());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
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
                throw new ConnectionException(e.Message);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
            catch (EmptyIdException e)
            {
                throw new EmptyIdException(e.Message);
            }
            catch (EmailNotFoundException e)
            {
                throw new EmailNotFoundException(e.Message);
            }
            catch (NotAuthorizedException e)
            {
                throw new NotAuthorizedException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}