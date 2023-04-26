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

namespace Luqmit3ish.ViewModels
{
    class SignupViewModel : ViewModelBase
    {
        public INavigation _navigation { get; set; }

        public ICommand signupClicked { protected set; get; }
        public ICommand LoginCommand { protected set; get; }
        public ICommand HidePasswordCommand { protected set; get; }
        public ICommand ShowPasswordCommand { protected set; get; }



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
        public SignupViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            signupClicked = new Command(async () => await OnSignupClicked());
            LoginCommand = new Command(async () => await OnLoginClicked());
            ShowPasswordCommand = new Command(OnUnHidePasswordClicked);
            HidePasswordCommand = new Command(OnHidePasswordClicked);
            userServices = new UserServices();
            OnInit();
        }

        private async Task OnSignupClicked()
        {
            try
            {
                if (isValidEmail(_email) && isValidPassword(_password) && isValidPhone(_phone) && _type != -1)
                {
                    SignUpRequest signUpRequest = new SignUpRequest()
                    {
                        Name = _name,
                        Email = _email,
                        Password = _password,
                        Phone = _phone,
                        Type = _selectedType.ToString()
                    };
                    Application.Current.MainPage = new VerificationPage(signUpRequest);
                }
                else if (!isValidEmail(_email))
                {
                    EmailErrorVisible = true;
                    EmailInValid = true;
                    EmailFrameColor = Color.DarkRed;
                }
                else if (!isValidPassword(_password))
                {
                    PasswordErrorVisible = true;
                    PasswordValid = false;
                    PasswordInvalid = true;
                    ShowPassword = false;
                    HidePassword = false;
                    IsPassword = true;
                    PasswordFrameColor = Color.DarkRed;
                }
                else if (!isValidPhone(_phone))
                {
                        PhoneErrorVisible = true;
                        PhoneValid = false;
                        PhoneInvalid = true;
                        PhoneFrameColor = Color.DarkRed;
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new PopUp("Please fill in all the required fields correctly."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                }
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

        #region NameField
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                _nameValid = true;
                _nameFrameColor = Color.Green;

                OnPropertyChanged(nameof(NameValid));
                OnPropertyChanged(nameof(NameFrameColor));
            }
        }
        private Color _nameFrameColor;
        public Color NameFrameColor
        {
            get => _nameFrameColor;
            set => SetProperty(ref _nameFrameColor, value);
        }
        private bool _nameValid;
        public bool NameValid
        {
            get => _nameValid;
            set => SetProperty(ref _nameValid, value);
        }
        #endregion

        #region EmailField
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);

                if (isValidEmail(_email))
                {
                    _emailErrorVisible = false;
                    _emailValid = true;
                    _emailInValid = false;
                    _emailFrameColor = Color.Green;
                    EmailErrorVisible = false;
                    EmailInValid = false;

                }
                OnPropertyChanged(nameof(EmailValid));
                OnPropertyChanged(nameof(EmailFrameColor));
                OnPropertyChanged(nameof(EmailErrorVisible));
                OnPropertyChanged(nameof(EmailInValid));
            }
        }
        private bool _emailErrorVisible = false;
        public bool EmailErrorVisible
        {
            get => _emailErrorVisible;
            set => SetProperty(ref _emailErrorVisible, value);
        }

        private bool _emailValid;
        public bool EmailValid
        {
            get => _emailValid;
            set => SetProperty(ref _emailValid, value);
        }

        private bool _emailInValid;
        public bool EmailInValid
        {
            get => _emailInValid;
            set => SetProperty(ref _emailInValid, value);
        }

        private bool isValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrEmpty(email)) return false;
            if (Regex.IsMatch(email, emailPattern))
            {
                return true;
            }
            return false;
        }

        private Color _emailFrameColor;
        public Color EmailFrameColor
        {
            get => _emailFrameColor;
            set => SetProperty(ref _emailFrameColor, value);
        }

        private string _emailErrorMessage = "Enter a valid email.";
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set => SetProperty(ref _emailErrorMessage, value);
        }
        #endregion



       //             _passwordValid = true;
      //              _passwordFrameColor = Color.Green;
       //             HidePassword = true;
        //            IsPassword = true;
        //            ShowPassword = false;
        //            PasswordErrorVisible = false;
         //           PasswordInvalid = false;

#region PasswordField
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);

                if (isValidPassword(_password))
                {

                    _passwordErrorVisible = false;
                    _passwordValid = true;
                    _passwordInvalid = false;
                    _passwordFrameColor = Color.Green;
                    _passwordErrorVisible = false;
                    _passwordInvalid = false;
                     HidePassword = true;
                     IsPassword = true;
                     ShowPassword = false;
                }
                OnPropertyChanged(nameof(PasswordErrorVisible));
                OnPropertyChanged(nameof(PasswordValid));
                OnPropertyChanged(nameof(PasswordInvalid));
                OnPropertyChanged(nameof(PasswordFrameColor));
                OnPropertyChanged(nameof(PasswordErrorMessage));
                OnPropertyChanged(nameof(HidePassword));
                OnPropertyChanged(nameof(IsPassword));
                OnPropertyChanged(nameof(ShowPassword));

            }
        }

        private bool _passwordErrorVisible = false;
        public bool PasswordErrorVisible
        {
            get => _passwordErrorVisible;
            set => SetProperty(ref _passwordErrorVisible, value);
        }

        private bool _passwordValid;
        public bool PasswordValid
        {
            get => _passwordValid;
            set => SetProperty(ref _passwordValid, value);
        }

        private bool _passwordInvalid;
        public bool PasswordInvalid
        {
            get => _passwordInvalid;
            set => SetProperty(ref _passwordInvalid, value);
        }

        private bool isValidPassword(string password)
        {
            string passwordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            if (string.IsNullOrEmpty(password)) return false;
            if (Regex.IsMatch(password, passwordPattern))
            {
                return true;
            }
            return false;
        }

        private Color _passwordFrameColor;
        public Color PasswordFrameColor
        {
            get => _passwordFrameColor;
            set => SetProperty(ref _passwordFrameColor, value);
        }

        private string _passwordErrorMessage = "Enter at least 8 characters including one number, one uppercase letter and a special character.";
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetProperty(ref _passwordErrorMessage, value);
        }
        #endregion

        #region PhoneField
        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                SetProperty(ref _phone, value);

                if (isValidPhone(_phone))
                {
                    _phoneFrameColor = Color.Green;
                    PhoneErrorVisible = false;
                    PhoneValid = true;
                    PhoneInvalid = false;

                }
                OnPropertyChanged(nameof(PhoneValid));
                OnPropertyChanged(nameof(PhoneFrameColor));
                OnPropertyChanged(nameof(PhoneErrorVisible));
                OnPropertyChanged(nameof(PhoneInvalid));

            }
        }

        private bool _phoneErrorVisible = false;
        public bool PhoneErrorVisible
        {
            get => _phoneErrorVisible;
            set => SetProperty(ref _phoneErrorVisible, value);
        }

        private bool _phoneValid;
        public bool PhoneValid
        {
            get => _phoneValid;
            set => SetProperty(ref _phoneValid, value);
        }

        private bool _phoneInvalid;
        public bool PhoneInvalid
        {
            get => _phoneInvalid;
            set => SetProperty(ref _phoneInvalid, value);
        }

        private bool isValidPhone(string phone)
        {
            string phonePattern = @"^(?:(?:(?:\+|00)970)|0)?5[69]\d{7}$";
            if (string.IsNullOrEmpty(phone)) return false;
            if (Regex.IsMatch(phone, phonePattern))
            {
                return true;
            }
            return false;
        }

        private Color _phoneFrameColor;
        public Color PhoneFrameColor
        {
            get => _phoneFrameColor;
            set => SetProperty(ref _phoneFrameColor, value);
        }

        private string _phoneErrorMessage = "Enter a valid phone number.";
        public string PhoneErrorMessage
        {
            get => _phoneErrorMessage;
            set => SetProperty(ref _phoneErrorMessage, value);
        }

        #endregion

        #region TypeField
        private int _type = -1;
        public int Type
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);

                if (_type != -1)
                {
                    _typeValid = true;
                    _typeFrameColor = Color.Green;
                }
                OnPropertyChanged(nameof(TypeValid));
                OnPropertyChanged(nameof(TypeFrameColor));

            }
        }

        private bool _typeValid;
        public bool TypeValid
        {
            get => _typeValid;
            set => SetProperty(ref _typeValid, value);
        }
        private Color _typeFrameColor;
        public Color TypeFrameColor
        {
            get => _typeFrameColor;
            set => SetProperty(ref _typeFrameColor, value);
        }


        private string _selectedType;
        public string SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value);
        }
        #endregion

        private readonly UserServices userServices;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }


        private async void OnInit()
        {
            try
            {
                Users = await userServices.GetUsers();
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

        public ICommand LoginClicked
        {
            get
            {
                return new Command(() =>
                {
                    _navigation.PushModalAsync(new LoginPage());
                });
            }
        }
    }
}