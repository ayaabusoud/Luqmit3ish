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
using Luqmit3ish.Services;

namespace Luqmit3ish.ViewModels
{
    class SignupViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SignupCommand { protected set; get; }
        public ICommand LoginCommand { protected set; get; }

        public SignupViewModel(INavigation navigation)
        {
            nameErrorMessage = "Please choose a username between 4 and 16 characters, using only letters (upper and lowercase), numbers, underscores, and hyphens.";
            emailErrorMessage = "Please enter a valid email address with a username and domain name separated by \"@\". The domain name should include at least one dot (.) and no spaces.";
            passwordErrorMessage = "Your password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.";
            confirmErrorMessage = "Password does not match.";
            phoneErrorMessage = "This phone number invalid.";
            location = type = -1;

            nameFrameColor = emailFrameColor = passwordFrameColor = confirmFrameColor = phoneFrameColor = locationFrameColor = typeFrameColor = Color.DarkGray;
            nameErrorVisible = emailErrorVisible = passwordErrorVisible = confirmErrorVisible = phoneErrorVisible = locationErrorVisible = typeErrorVisible = false;
            this.Navigation = navigation;
            SignupCommand = new Command(async () => await OnSignupClicked());
            LoginCommand = new Command(async () => await OnLoginClicked());

            userServices = new UserServices();
            OnInit();

        }


        private async Task OnSignupClicked()
        {
            await Navigation.PushModalAsync(new VerificationPage());
        }
        private async Task OnLoginClicked()
        {
            Navigation.PushModalAsync(new LoginPage());
        }

        public void OnPropertyChanged(string PrpertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PrpertyName));
        }

        #region NameField
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged(nameof(Name));

                if (isValidName(name))
                {
                    nameErrorVisible = false;
                    nameValid = true;
                    nameInvalid = false;
                    nameFrameColor = Color.Green;
                }
                else
                {
                    nameErrorVisible = true;
                    nameValid = false;
                    nameInvalid = true;
                    nameFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(NameErrorVisible));
                OnPropertyChanged(nameof(NameValid));
                OnPropertyChanged(nameof(NameInvalid));
                OnPropertyChanged(nameof(NameFrameColor));
                OnPropertyChanged(nameof(NameErrorMessage));

            }
        }

        private bool nameErrorVisible;
        public bool NameErrorVisible
        {
            get => nameErrorVisible;
            set
            {
                if (nameErrorVisible == value) return;
                nameErrorVisible = value;
            }
        }

        private bool nameValid;
        public bool NameValid
        {
            get => nameValid;
            set
            {
                if (nameValid == value) return;
                nameValid = value;
            }
        }

        private bool nameInvalid;
        public bool NameInvalid
        {
            get => nameInvalid;
            set
            {
                if (nameInvalid == value) return;
                nameInvalid = value;
            }
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

        private Color nameFrameColor;
        public Color NameFrameColor
        {
            get => nameFrameColor;
            set
            {
                if (nameFrameColor == value) return;

                nameFrameColor = value;
            }
        }

        private string nameErrorMessage;
        public string NameErrorMessage
        {
            get => nameErrorMessage;
            set
            {
                if (nameErrorMessage == value) return;
                nameErrorMessage = value;
            }
        }
        #endregion

        #region EmailField
        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (email == value) return;
                email = value;
                OnPropertyChanged(nameof(Email));

                if (isValidEmail(email))
                {
                    emailErrorVisible = false;
                    emailValid = true;
                    emailInValid = false;
                    emailFrameColor = Color.Green;
                }
                else
                {
                    emailErrorVisible = true;
                    emailValid = false;
                    emailInValid = true;
                    emailFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(EmailErrorVisible));
                OnPropertyChanged(nameof(EmailValid));
                OnPropertyChanged(nameof(EmailInValid));
                OnPropertyChanged(nameof(EmailFrameColor));
                OnPropertyChanged(nameof(EmailErrorMessage));

            }
        }

        private bool emailErrorVisible;
        public bool EmailErrorVisible
        {
            get => emailErrorVisible;
            set
            {
                if (emailErrorVisible == value) return;
                emailErrorVisible = value;
            }
        }

        private bool emailValid;
        public bool EmailValid
        {
            get => emailValid;
            set
            {
                if (emailValid == value) return;
                emailValid = value;
            }
        }

        private bool emailInValid;
        public bool EmailInValid
        {
            get => emailInValid;
            set
            {
                if (emailInValid == value) return;
                emailInValid = value;
            }
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

        private Color emailFrameColor;
        public Color EmailFrameColor
        {
            get => emailFrameColor;
            set
            {
                if (emailFrameColor == value) return;

                emailFrameColor = value;
            }
        }

        private string emailErrorMessage;
        public string EmailErrorMessage
        {
            get => emailErrorMessage;
            set
            {
                if (emailErrorMessage == value) return;
                emailErrorMessage = value;
            }
        }
        #endregion

        #region PasswordField
        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (password == value) return;
                password = value;
                OnPropertyChanged(nameof(Password));

                if (isValidPassword(password))
                {
                   
                    passwordErrorVisible = false;
                    passwordValid = true;
                    passwordInvalid = false;
                    passwordFrameColor = Color.Green;
                }
                else
                {
                    passwordErrorVisible = true;
                    passwordValid = false;
                    passwordInvalid = true;
                    passwordFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(PasswordErrorVisible));
                OnPropertyChanged(nameof(PasswordValid));
                OnPropertyChanged(nameof(PasswordInvalid));
                OnPropertyChanged(nameof(PasswordFrameColor));
                OnPropertyChanged(nameof(PasswordErrorMessage));

            }
        }

        private bool passwordErrorVisible;
        public bool PasswordErrorVisible
        {
            get => passwordErrorVisible;
            set
            {
                if (passwordErrorVisible == value) return;
                passwordErrorVisible = value;
            }
        }

        private bool passwordValid;
        public bool PasswordValid
        {
            get => passwordValid;
            set
            {
                if (passwordValid == value) return;
                passwordValid = value;
            }
        }

        private bool passwordInvalid;
        public bool PasswordInvalid
        {
            get => passwordInvalid;
            set
            {
                if (passwordInvalid == value) return;
                passwordInvalid = value;
            }
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

        private Color passwordFrameColor;
        public Color PasswordFrameColor
        {
            get => passwordFrameColor;
            set
            {
                if (passwordFrameColor == value) return;

                passwordFrameColor = value;
            }
        }

        private string passwordErrorMessage;
        public string PasswordErrorMessage
        {
            get => passwordErrorMessage;
            set
            {
                if (passwordErrorMessage == value) return;
                passwordErrorMessage = value;
            }
        }
        #endregion

        #region ConfirmField
        private string confirm;
        public string Confirm
        {
            get => confirm;
            set
            {
                if (confirm == value) return;
                confirm = value;
                OnPropertyChanged(nameof(Confirm));

                if (isValidConfirm(confirm) && isValidPassword(confirm))
                {
                    confirmErrorVisible = false;
                    confirmValid = true;
                    confirmInvalid = false;
                    confirmFrameColor = Color.Green;
                }
                else
                {
                    confirmErrorVisible = true;
                    confirmValid = false;
                    confirmInvalid = true;
                    confirmFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(ConfirmErrorVisible));
                OnPropertyChanged(nameof(ConfirmValid));
                OnPropertyChanged(nameof(ConfirmInvalid));
                OnPropertyChanged(nameof(ConfirmFrameColor));
                OnPropertyChanged(nameof(ConfirmErrorMessage));

            }
        }

        private bool confirmErrorVisible;
        public bool ConfirmErrorVisible
        {
            get => confirmErrorVisible;
            set
            {
                if (confirmErrorVisible == value) return;
                confirmErrorVisible = value;
            }
        }

        private bool confirmValid;
        public bool ConfirmValid
        {
            get => confirmValid;
            set
            {
                if (confirmValid == value) return;
                confirmValid = value;
            }
        }

        private bool confirmInvalid;
        public bool ConfirmInvalid
        {
            get => confirmInvalid;
            set
            {
                if (confirmInvalid == value) return;
                confirmInvalid = value;
            }
        }

        private bool isValidConfirm(string confirm)
        {
            if (string.IsNullOrEmpty(confirm)) return false;

            return confirm.Equals(password);
        }

        private Color confirmFrameColor;
        public Color ConfirmFrameColor
        {
            get => confirmFrameColor;
            set
            {
                if (confirmFrameColor == value) return;

                confirmFrameColor = value;
            }
        }

        private string confirmErrorMessage;
        public string ConfirmErrorMessage
        {
            get => confirmErrorMessage;
            set
            {
                if (confirmErrorMessage == value) return;
                confirmErrorMessage = value;
            }
        }
        #endregion

        #region PhoneField
        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                if (phone == value) return;
                phone = value;
                OnPropertyChanged(nameof(Phone));

                if (isValidPhone(phone))
                {
                    phoneErrorVisible = false;
                    phoneValid = true;
                    phoneInvalid = false;
                    phoneFrameColor = Color.Green;
                }
                else
                {
                    phoneErrorVisible = true;
                    phoneValid = false;
                    phoneInvalid = true;
                    phoneFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(PhoneErrorVisible));
                OnPropertyChanged(nameof(PhoneValid));
                OnPropertyChanged(nameof(PhoneInvalid));
                OnPropertyChanged(nameof(PhoneFrameColor));
                OnPropertyChanged(nameof(PhoneErrorMessage));

            }
        }

        private bool phoneErrorVisible;
        public bool PhoneErrorVisible
        {
            get => phoneErrorVisible;
            set
            {
                if (phoneErrorVisible == value) return;
                phoneErrorVisible = value;
            }
        }

        private bool phoneValid;
        public bool PhoneValid
        {
            get => phoneValid;
            set
            {
                if (phoneValid == value) return;
                phoneValid = value;
            }
        }

        private bool phoneInvalid;
        public bool PhoneInvalid
        {
            get => phoneInvalid;
            set
            {
                if (phoneInvalid == value) return;
                phoneInvalid = value;
            }
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

        private Color phoneFrameColor;
        public Color PhoneFrameColor
        {
            get => phoneFrameColor;
            set
            {
                if (phoneFrameColor == value) return;

                phoneFrameColor = value;
            }
        }

        private string phoneErrorMessage;
        public string PhoneErrorMessage
        {
            get => phoneErrorMessage;
            set
            {
                if (phoneErrorMessage == value) return;
                phoneErrorMessage = value;
            }
        }

        #endregion

        #region LocationField
        private int location;
        public int Location
        {
            get => location;
            set
            {
                if (location == value) return;
                location = value;
                OnPropertyChanged(nameof(Location));

                if (location != -1)
                {
                    locationErrorVisible = false;
                    locationValid = true;
                    locationInvalid = false;
                    locationFrameColor = Color.Green;
                }
                else
                {
                    locationErrorVisible = true;
                    locationValid = false;
                    locationInvalid = true;
                    locationFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(LocationErrorVisible));
                OnPropertyChanged(nameof(LocationValid));
                OnPropertyChanged(nameof(LocationInvalid));
                OnPropertyChanged(nameof(LocationFrameColor));
                OnPropertyChanged(nameof(LocationErrorMessage));

            }
        }

        private bool locationErrorVisible;
        public bool LocationErrorVisible
        {
            get => locationErrorVisible;
            set
            {
                if (locationErrorVisible == value) return;
                locationErrorVisible = value;
            }
        }

        private bool locationValid;
        public bool LocationValid
        {
            get => locationValid;
            set
            {
                if (locationValid == value) return;
                locationValid = value;
            }
        }

        private bool locationInvalid;
        public bool LocationInvalid
        {
            get => locationInvalid;
            set
            {
                if (locationInvalid == value) return;
                locationInvalid = value;
            }
        }

        private Color locationFrameColor;
        public Color LocationFrameColor
        {
            get => locationFrameColor;
            set
            {
                if (locationFrameColor == value) return;

                locationFrameColor = value;
            }
        }

        private string locationErrorMessage;
        public string LocationErrorMessage
        {
            get => locationErrorMessage;
            set
            {
                if (locationErrorMessage == value) return;
                locationErrorMessage = value;
            }
        }

        private string selectedLocation;
        public string SelectedLocation {
            get => selectedLocation;
            set {
                if (selectedLocation == value) return;
                selectedLocation = value;
            }
        }
        #endregion

        #region TypeField
        private int type;
        public int Ttype
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged(nameof(Ttype));

                if (type != -1)
                {
                    typeErrorVisible = false;
                    typeValid = true;
                    typeInvalid = false;
                    typeFrameColor = Color.Green;
                }
                else
                {
                    typeErrorVisible = true;
                    typeValid = false;
                    typeInvalid = true;
                    typeFrameColor = Color.Red;
                }
                OnPropertyChanged(nameof(TtypeErrorVisible));
                OnPropertyChanged(nameof(TtypeValid));
                OnPropertyChanged(nameof(TtypeInvalid));
                OnPropertyChanged(nameof(TtypeFrameColor));
                OnPropertyChanged(nameof(TtypeErrorMessage));

            }
        }

        private bool typeErrorVisible;
        public bool TtypeErrorVisible
        {
            get => typeErrorVisible;
            set
            {
                if (typeErrorVisible == value) return;
                typeErrorVisible = value;
            }
        }

        private bool typeValid;
        public bool TtypeValid
        {
            get => typeValid;
            set
            {
                if (typeValid == value) return;
                typeValid = value;
            }
        }

        private bool typeInvalid;
        public bool TtypeInvalid
        {
            get => typeInvalid;
            set
            {
                if (typeInvalid == value) return;
                typeInvalid = value;
            }
        }

        private Color typeFrameColor;
        public Color TtypeFrameColor
        {
            get => typeFrameColor;
            set
            {
                if (typeFrameColor == value) return;

                typeFrameColor = value;
            }
        }

        private string typeErrorMessage;
        public string TtypeErrorMessage
        {
            get => typeErrorMessage;
            set
            {
                if (typeErrorMessage == value) return;
                typeErrorMessage = value;
            }
        }

        private string selectedType;
        public string SelectedType
        {
            get => selectedType;
            set
            {
                if (selectedType == value) return;
                selectedType = value;
            }
        }
        #endregion

        public ICommand signupClicked
        {
            get
            {
                return new Command(()=>
                {
                    if (isValidName(name) && isValidEmail(email) && isValidPassword(password) && isValidConfirm(confirm) && isValidPhone(phone) && location != -1 && type != -1)
                    {
                        User newUser = new User()
                        {
                            Name = name,
                            Email = email,
                            Password = password,
                            Phone = phone,
                            Location = selectedLocation.ToString(),
                            Type = selectedType.ToString()
                        };

                        var res = InsertNewUser(newUser);
                        Console.WriteLine("res = " + res);

                        
                        Console.WriteLine("Doneeeee");
                        Application.Current.MainPage = new AppShellCharity();

                    }

                });
            }
        }


        public async Task<bool> InsertNewUser(User newUser)
        {
            bool isInserted = await userServices.InsertUser(newUser);

            if (isInserted)
            {
                Navigation.PushModalAsync(new VerificationPage());
                return true;
            }

            return false;
        }

        private readonly UserServices userServices;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set {
                if (_users == value) return;
                _users = value;
            }
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
                    Navigation.PushModalAsync(new LoginPage());
                });
            }
        }
    }
}
