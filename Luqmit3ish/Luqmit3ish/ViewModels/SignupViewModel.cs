using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;
using System.Diagnostics;
using Luqmit3ish.Exceptions;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Luqmit3ish.ViewModels
{
    class SignupViewModel : ViewModelBase
    {
        public INavigation _navigation { get; set; }

        public ICommand signupClicked { protected set; get; }
        public ICommand LoginCommand { protected set; get; }

        public SignupViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            signupClicked = new Command(async () => await OnSignupClicked());
            LoginCommand = new Command(async () => await OnLoginClicked());

            userServices = new UserServices();
            OnInit();
        }

        private async Task OnSignupClicked()
        {
            try
            {
                if (isValidName(_name) && isValidEmail(_email) && isValidPassword(_password) && isValidConfirm(_confirm) && isValidPhone(_phone) && _location != -1 && _type != -1)
                {
                    SignUpRequest signUpRequest = new SignUpRequest()
                    {
                        Name = _name,
                        Email = _email,
                        Password = _password,
                        Phone = _phone,
                        Location = _selectedLocation.ToString(),
                        Type = _selectedType.ToString()
                    };
                     Application.Current.MainPage = new VerificationPage(signUpRequest);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please fill in all the required fields correctly.", "OK");
                }
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await App.Current.MainPage.DisplayAlert("Error", "There was a problem with your internet connection.", "OK");
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await App.Current.MainPage.DisplayAlert("Error", "Unable to connect to the server. Please check your internet connection and try again.", "OK");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await App.Current.MainPage.DisplayAlert("Error", "An unexpected error has occurred. Please try again later.", "OK");

            }
        }

        private async Task OnLoginClicked()
        {
            await _navigation.PushModalAsync(new LoginPage());
        }

        #region NameField
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);

                if (isValidName(_name))
                {
                    _nameErrorVisible = false;
                    _nameValid = true;
                    _nameInvalid = false;
                    _nameFrameColor = Color.Green;
                }
                else
                {
                    _nameErrorVisible = true;
                    _nameValid = false;
                    _nameInvalid = true;
                    _nameFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(NameErrorVisible));
                OnPropertyChanged(nameof(NameValid));
                OnPropertyChanged(nameof(NameInvalid));
                OnPropertyChanged(nameof(NameFrameColor));
                OnPropertyChanged(nameof(NameErrorMessage));
            }
        }

        private bool _nameErrorVisible = false;
        public bool NameErrorVisible
        {
            get => _nameErrorVisible;
            set => SetProperty(ref _nameErrorVisible, value);
        }

        private bool _nameValid;
        public bool NameValid
        {
            get => _nameValid;
            set => SetProperty(ref _nameValid, value);
        }

        private bool _nameInvalid;
        public bool NameInvalid
        {
            get => _nameInvalid;
            set => SetProperty(ref _nameInvalid, value);
        }

        private bool isValidName(string name)
        {
            string namePattern = @"^[a-zA-Z0-9_-]{4,16}$";
            if (string.IsNullOrEmpty(name)) {
                return false;
            }

            if (Regex.IsMatch(name, namePattern))
            {
                return true;
            }
            return false;
        }

        private Color _nameFrameColor;
        public Color NameFrameColor
        {
            get => _nameFrameColor;
            set => SetProperty(ref _nameFrameColor, value);
        }

        private string _nameErrorMessage = "Please choose a username between 4 and 16 characters, using only letters (upper and lowercase), numbers, underscores, and hyphens.";
        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set => SetProperty(ref _nameErrorMessage, value);
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
                }
                else
                {
                    _emailErrorVisible = true;
                    _emailValid = false;
                    _emailInValid = true;
                    _emailFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(EmailErrorVisible));
                OnPropertyChanged(nameof(EmailValid));
                OnPropertyChanged(nameof(EmailInValid));
                OnPropertyChanged(nameof(EmailFrameColor));
                OnPropertyChanged(nameof(EmailErrorMessage));

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

        private string _emailErrorMessage = "Please enter a valid email address with a username and domain name separated by \"@\". The domain name should include at least one dot (.) and no spaces.";
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set => SetProperty(ref _emailErrorMessage, value);
        }
        #endregion

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
                }
                else
                {
                    _passwordErrorVisible = true;
                    _passwordValid = false;
                    _passwordInvalid = true;
                    _passwordFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(PasswordErrorVisible));
                OnPropertyChanged(nameof(PasswordValid));
                OnPropertyChanged(nameof(PasswordInvalid));
                OnPropertyChanged(nameof(PasswordFrameColor));
                OnPropertyChanged(nameof(PasswordErrorMessage));

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

        private string _passwordErrorMessage = "Your password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.";
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetProperty(ref _passwordErrorMessage, value);
        }
        #endregion

        #region ConfirmField
        private string _confirm;
        public string Confirm
        {
            get => _confirm;
            set
            {
                SetProperty(ref _confirm, value);

                if (isValidConfirm(_confirm) && isValidPassword(_confirm))
                {
                    _confirmErrorVisible = false;
                    _confirmValid = true;
                    _confirmInvalid = false;
                    _confirmFrameColor = Color.Green;
                }
                else
                {
                    _confirmErrorVisible = true;
                    _confirmValid = false;
                    _confirmInvalid = true;
                    _confirmFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(ConfirmErrorVisible));
                OnPropertyChanged(nameof(ConfirmValid));
                OnPropertyChanged(nameof(ConfirmInvalid));
                OnPropertyChanged(nameof(ConfirmFrameColor));
                OnPropertyChanged(nameof(ConfirmErrorMessage));

            }
        }

        private bool _confirmErrorVisible = false;
        public bool ConfirmErrorVisible
        {
            get => _confirmErrorVisible;
            set => SetProperty(ref _confirmErrorVisible, value);
        }

        private bool _confirmValid;
        public bool ConfirmValid
        {
            get => _confirmValid;
            set => SetProperty(ref _confirmValid, value);
        }

        private bool _confirmInvalid;
        public bool ConfirmInvalid
        {
            get => _confirmInvalid;
            set => SetProperty(ref _confirmInvalid, value);
        }

        private bool isValidConfirm(string confirm)
        {
            if (string.IsNullOrEmpty(confirm)) return false;

            return confirm.Equals(_password);
        }

        private Color _confirmFrameColor;
        public Color ConfirmFrameColor
        {
            get => _confirmFrameColor;
            set => SetProperty(ref _confirmFrameColor, value);
        }

        private string _confirmErrorMessage = "Password does not match.";
        public string ConfirmErrorMessage
        {
            get => _confirmErrorMessage;
            set => SetProperty(ref _confirmErrorMessage, value);
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
                    _phoneErrorVisible = false;
                    _phoneValid = true;
                    _phoneInvalid = false;
                    _phoneFrameColor = Color.Green;
                }
                else
                {
                    _phoneErrorVisible = true;
                    _phoneValid = false;
                    _phoneInvalid = true;
                    _phoneFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(PhoneErrorVisible));
                OnPropertyChanged(nameof(PhoneValid));
                OnPropertyChanged(nameof(PhoneInvalid));
                OnPropertyChanged(nameof(PhoneFrameColor));
                OnPropertyChanged(nameof(PhoneErrorMessage));

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

        private string _phoneErrorMessage = "This phone number invalid.";
        public string PhoneErrorMessage
        {
            get => _phoneErrorMessage;
            set => SetProperty(ref _phoneErrorMessage, value);
        }

        #endregion

        #region LocationField
        private int _location = -1; 
        public int Location
        {
            get => _location;
            set
            {
                SetProperty(ref _location, value);

                if (_location != -1)
                {
                    _locationErrorVisible = false;
                    _locationValid = true;
                    _locationInvalid = false;
                    _locationFrameColor = Color.Green;
                }
                else
                {
                    _locationErrorVisible = true;
                    _locationValid = false;
                    _locationInvalid = true;
                    _locationFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(LocationErrorVisible));
                OnPropertyChanged(nameof(LocationValid));
                OnPropertyChanged(nameof(LocationInvalid));
                OnPropertyChanged(nameof(LocationFrameColor));
                OnPropertyChanged(nameof(LocationErrorMessage));

            }
        }

        private bool _locationErrorVisible = false;
        public bool LocationErrorVisible
        {
            get => _locationErrorVisible;
            set => SetProperty(ref _locationErrorVisible, value);
        }

        private bool _locationValid;
        public bool LocationValid
        {
            get => _locationValid;
            set => SetProperty(ref _locationValid, value);
        }

        private bool _locationInvalid;
        public bool LocationInvalid
        {
            get => _locationInvalid;
            set => SetProperty(ref _locationInvalid, value);
        }

        private Color _locationFrameColor;
        public Color LocationFrameColor
        {
            get => _locationFrameColor;
            set => SetProperty(ref _locationFrameColor, value);
        }

        private string _locationErrorMessage;
        public string LocationErrorMessage
        {
            get => _locationErrorMessage;
            set => SetProperty(ref _locationErrorMessage, value);
        }

        private string _selectedLocation;
        public string SelectedLocation {
            get => _selectedLocation;
            set => SetProperty(ref _selectedLocation, value);
        }
        #endregion

        #region TypeField
        private int _type = -1;
        public int Ttype
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);

                if (_type != -1)
                {
                    _typeErrorVisible = false;
                    _typeValid = true;
                    _typeInvalid = false;
                    _typeFrameColor = Color.Green;
                }
                else
                {
                    _typeErrorVisible = true;
                    _typeValid = false;
                    _typeInvalid = true;
                    _typeFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(TtypeErrorVisible));
                OnPropertyChanged(nameof(TtypeValid));
                OnPropertyChanged(nameof(TtypeInvalid));
                OnPropertyChanged(nameof(TtypeFrameColor));
                OnPropertyChanged(nameof(TtypeErrorMessage));

            }
        }

        private bool _typeErrorVisible = false;
        public bool TtypeErrorVisible
        {
            get => _typeErrorVisible;
            set => SetProperty(ref _typeErrorVisible, value);
        }

        private bool _typeValid;
        public bool TtypeValid
        {
            get => _typeValid;
            set => SetProperty(ref _typeValid, value);
        }

        private bool _typeInvalid;
        public bool TtypeInvalid
        {
            get => _typeInvalid;
            set => SetProperty(ref _typeInvalid, value);
        }

        private Color _typeFrameColor;
        public Color TtypeFrameColor
        {
            get => _typeFrameColor;
            set => SetProperty(ref _typeFrameColor, value);
        }

        private string _typeErrorMessage;
        public string TtypeErrorMessage
        {
            get => _typeErrorMessage;
            set => SetProperty(ref _typeErrorMessage, value);
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
            Users = await userServices.GetUsers();
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
