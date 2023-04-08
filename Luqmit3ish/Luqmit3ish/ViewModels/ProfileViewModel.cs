using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Net.WebRequestMethods;


namespace Luqmit3ish.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        public INavigation Navigation { get; set; }
        public ICommand EditCommand { protected set; get; }
        public ICommand DoneCommand { protected set; get; }
        public ICommand CancelCommand { protected set; get; }

        public const string DEFULY_IMAGE= "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
       
        public ProfileViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            EditCommand = new Command(async () => await OnEditClicked());
            DoneCommand = new Command(async () => await OnDoneClicked());
            CancelCommand=new Command(async () => await OnCancelClicked());
            userServices = new UserServices();
            OnInit();
        }

        private User _userInfo;
        public User UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }
        public UserServices userServices;
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                if (Regex.IsMatch(value, "^[^@]+@[^\\.]+\\..+$"))
                {
                    EmailErrorVisible = false;
                }
                else
                {
                    EmailErrorVisible = true;
                    EmailErrorMessage = "Invalid email address";
                }
            }
        }

        private bool _emailErrorVisible;
        public bool EmailErrorVisible
        {
            get => _emailErrorVisible;
            set => SetProperty(ref _emailErrorVisible, value);
        }
        
        private string _emailErrorMessage;
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set => SetProperty(ref _emailErrorMessage, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                SetProperty(ref _phone, value);
                if (Regex.IsMatch(value, "^[0-9]{8,10}$"))
                {
                    PhoneErrorVisible = false;
                }
                else
                {
                    PhoneErrorVisible = true;
                    PhoneErrorMessage = "Phone number must be 8-10 digits";
                }
            }
        }

        private bool _phoneErrorVisible;
        public bool PhoneErrorVisible
        {
            get => _phoneErrorVisible;
            set => SetProperty(ref _phoneErrorVisible, value);
        }

        private string _phoneErrorMessage;
        public string PhoneErrorMessage
        {
            get => _phoneErrorMessage;
            set => SetProperty(ref _phoneErrorMessage, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                if (Regex.IsMatch(value, "^[a-zA-Z0-9 ]*$"))
                {
                    NameErrorVisible = false;
                }
                else
                {
                    NameErrorVisible = true;
                    NameErrorMessage = "Name can only contain letters,numbers and spaces";
                }
            }
        }

        private bool _nameErrorVisible;
        public bool NameErrorVisible
        {
            get => _nameErrorVisible;
            set => SetProperty(ref _nameErrorVisible, value);
        }

        private string _nameErrorMessage;
        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set => SetProperty(ref _nameErrorMessage, value);
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                SetProperty(ref _location, value);
                if (Regex.IsMatch(value, "^[a-zA-Z0-9 ]*$"))
                {
                    LocationErrorVisible = false;

                }

                else
                {
                    LocationErrorVisible = true;
                    LocationErrorMessage = "Location can only contain letters, numbers, and spaces";
                }
            }
        }
        private string _photo;
        public string Photo
        {
            get=> _photo;
            set
            {
                SetProperty(ref _photo, value);
            }
        }
       
        private bool _locationErrorVisible;
        public bool LocationErrorVisible
        {
            get => _locationErrorVisible;
            set => SetProperty(ref _locationErrorVisible, value);
        }

        private string _locationErrorMessage;
        public string LocationErrorMessage
        {
            get => _locationErrorMessage;
            set => SetProperty(ref _locationErrorMessage, value);
        }

        private bool _edit = false;
        public bool EditEnable
        {
            get => _edit;
            set => SetProperty(ref _edit, value);
        }
        private bool _viewEnable = true;
        public bool ViewEnable
        {
            get => _viewEnable;
            set => SetProperty(ref _viewEnable, value);
        }

     

        private async void OnInit()
        {
            string email;
            Photo = DEFULY_IMAGE;

            try
            {
              email = Preferences.Get("userEmail", null);
                if (string.IsNullOrEmpty(email))
                {
                    throw new Exception("Email not found in Preferences");
                }
                UserInfo = await userServices.GetUserByEmail(email);


            }
      
            catch (Exception)
            {
                throw new Exception("Get User Filed");
            }



            if (UserInfo != null)
            {
                if (UserInfo.Photo == null)
                {
                    Photo = DEFULY_IMAGE;
                }
                else
                {
                    Photo = UserInfo.Photo;
                }
                Email = UserInfo.Email;
                Phone = UserInfo.Phone;
                Name = UserInfo.Name;
                Location = UserInfo.Location;
            }
        }
        private async Task OnDoneClicked()
        {
            string email;


            try
            {
                email = Preferences.Get("userEmail", null);
                if (string.IsNullOrEmpty(email))
                {
                    throw new Exception("Email not found in Preferences");
                }
                UserInfo = await userServices.GetUserByEmail(email);

            }
            catch (Exception e )
            {
                Debug.WriteLine(e.Message);

            }
       
            if (UserInfo != null)
            {
                if (UserInfo.Photo == null)
                {
                    Photo = DEFULY_IMAGE;
                }
                else
                {
                    Photo = UserInfo.Photo;
                }

                User user = new User
                {
                    id = UserInfo.id,
                    Name = Name,
                    Email = Email,
                    Location = Location,
                    Phone = Phone,
                    Photo = UserInfo.Photo,
                    Type = UserInfo.Type,
                    Password = UserInfo.Password
                };
                if ((EmailErrorVisible || PhoneErrorVisible || LocationErrorVisible || NameErrorVisible)) {
                    await Application.Current.MainPage.DisplayAlert("Warning", "Please make sure all information is valid before saving changes.", "OK");
                    return;
                }
                try
                {
                    await userServices.EditProfile(user);
                    EditEnable = false;
                    ViewEnable = true;
                }
                

            catch (ArgumentException e)
                {
                    Debug.WriteLine(e.Message);
                }
                catch (Exception  e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            
        }
        private async Task OnCancelClicked()
        {
            string email;
            Photo = DEFULY_IMAGE;

            try
            {
                email = Preferences.Get("userEmail", null);
                if (string.IsNullOrEmpty(email))
                {
                    throw new Exception("Email not found in Preferences");
                }
                UserInfo = await userServices.GetUserByEmail(email);

            }
        
            catch (Exception)
            {
                throw new Exception("Get User Filed");
            }



            if (UserInfo != null)
            {
                if (UserInfo.Photo == null)
                {
                    Photo = DEFULY_IMAGE;
                }
                else
                {
                    Photo = UserInfo.Photo;
                }
                Email = UserInfo.Email;
                Phone = UserInfo.Phone;
                Name = UserInfo.Name;
                Location = UserInfo.Location;
            }
            ViewEnable = true;
            EditEnable = false;
        }

        private async Task OnEditClicked()
        {
           EditEnable = true;
           ViewEnable = false;
        }
      
    


    }
}
