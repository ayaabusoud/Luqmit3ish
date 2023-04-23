using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        public ICommand EditCommand { protected set; get; }
        public ICommand DoneCommand { protected set; get; }
        public ICommand CancelCommand { protected set; get; }

       
        public ProfileViewModel(INavigation navigation, User UserInfo)
        {
            EditCommand = new Command( OnEditClicked);
            DoneCommand = new Command(async () => await OnDoneClicked());
            CancelCommand=new Command(OnCancelClicked);
            userServices = new UserServices();
            this._userInfo = UserInfo;
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

     

        private void OnInit()
        {
            if (_userInfo != null)
                {
                    Email = _userInfo.Email;
                    Phone = _userInfo.Phone;
                    Name = _userInfo.Name;
                    Location = _userInfo.Location;
            }  
        }

        private async Task OnDoneClicked()
        {
            User user = new User
            {
                Id = _userInfo.Id,
                Name = Name,
                Email = Email,
                Location = Location,
                Phone = Phone,
                Photo = UserInfo.Photo,
                Type = _userInfo.Type,
                Password = _userInfo.Password
            };

            if (EmailErrorVisible || PhoneErrorVisible || LocationErrorVisible || NameErrorVisible)
                    {
                await PopupNavigation.Instance.PushAsync(new PopUp("Please make sure all information is valid before saving changes."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
                        return;
                    }

            try
            {
                await userServices.EditProfile(user);
                
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
            EditEnable = false;
            ViewEnable = true;
        }
            
        
        private void OnCancelClicked()
        {

           Email = _userInfo.Email;
           Phone = _userInfo.Phone;
           Name = _userInfo.Name;
           Location = _userInfo.Location;
                
           ViewEnable = true;
           EditEnable = false;

        }

        private void OnEditClicked()
        {
           EditEnable = true;
           ViewEnable = false;
        }
      
    


    }
}
