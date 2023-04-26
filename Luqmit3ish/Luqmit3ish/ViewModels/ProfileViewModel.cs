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
using static System.Net.WebRequestMethods;


namespace Luqmit3ish.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand EditCommand { protected set; get; }
        public ICommand DoneCommand { protected set; get; }
        public ICommand CancelCommand { protected set; get; }
        public ICommand EditPhotoClicked { protected set; get; }
        public string ConstName { protected set; get; }
        private User _userInfo;

        public ProfileViewModel(INavigation navigation, User userInfo)
        {
            this._navigation = navigation;
            _userInfo = userInfo;
            ConstName = userInfo.Name;
            DoneCommand = new Command(async () => await OnDoneClicked());
            CancelCommand = new Command(async () => await OnCancelClicked());
            EditPhotoClicked = new Command(async () => await OnEditPhotoClicked());
            userServices = new UserServices();
            OnInit();
        }

        private async Task OnEditPhotoClicked()
        {
            try
            {
                string photoPath = string.Empty;
                await Permissions.RequestAsync<Permissions.Photos>();

                var result = await MediaPicker.PickPhotoAsync();

                if (result != null)
                {
                    photoPath = result.FullPath;
                    _photo = photoPath;
                    OnPropertyChanged(nameof(Photo));
                }
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await Application.Current.MainPage.DisplayAlert("Bad Request", "Please check your connection", "Ok");
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await Application.Current.MainPage.DisplayAlert("Error", "Something went bad on this reservation, you can try again", "Ok");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }



        public UserServices userServices;
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    ErrorVisible = true;
                    ErrorMessage = "Email address is required";
                }
                else if (Regex.IsMatch(value, "^[^@]+@[^\\.]+\\..+$"))
                {
                    ErrorVisible = false;
                }
                else
                {
                    ErrorVisible = true;
                    ErrorMessage = "Invalid email address";
                }
            }
        }


        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                SetProperty(ref _phone, value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    ErrorVisible = true;
                    ErrorMessage = "Phone is required";
                }
                else if (Regex.IsMatch(value, "^[0-9]{8,10}$"))
                {
                    ErrorVisible = false;
                }
                else
                {
                    ErrorVisible = true;
                    ErrorMessage = "Phone number must be 8-10 digits";
                }
            }
        }


        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }


        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    ErrorVisible = true;
                    ErrorMessage = "Name is required";
                }
                else if (Regex.IsMatch(value, "^[a-zA-Z0-9 ]*$"))
                {
                    ErrorVisible = false;
                }
                else
                {
                    ErrorVisible = true;
                    ErrorMessage = "Name can only contain letters,numbers and spaces";
                }
            }
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                SetProperty(ref _location, value);
                if (Regex.IsMatch(value, "^[a-zA-Z0-9]*$"))
                {
                    ErrorVisible = false;
                }

                else
                {
                    ErrorVisible = true;
                    ErrorMessage = "Location can only contain letters, numbers, and spaces";
                }
            }
        }
        private string _photo;
        public string Photo
        {
            get => _photo;
            set
            {
                SetProperty(ref _photo, value);
            }
        }

        private bool _errorVisible;
        public bool ErrorVisible
        {
            get => _errorVisible;
            set => SetProperty(ref _errorVisible, value);
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
            try
            {


                Task.Run(async () =>
                {
                    string email;

                    try
                    {
                        email = Preferences.Get("userEmail", null);
                        if (string.IsNullOrEmpty(email))
                        {
                            return;
                        }

                        if (_userInfo != null)
                        {
                            Email = _userInfo.Email;
                            Phone = _userInfo.Phone;
                            Name = _userInfo.Name;
                            Location = _userInfo.Location;
                            Photo = _userInfo.Photo;
                        }
                    }
                    catch (ConnectionException)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "There was a connection error. Please check your internet connection and try again.", "OK");
                    }
                    catch (HttpRequestException)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "There was an HTTP request error. Please try again later.", "OK");

                    }
                    catch (Exception)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "An error occurred. Please try again later.", "OK");
                    }

                }).Wait();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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
                    return;
                }

                if (_userInfo != null)
                {
                    User user = new User
                    {
                        Id = _userInfo.Id,
                        Name = Name,
                        Email = _userInfo.Email,
                        Location = Location,
                        Phone = Phone,
                        Photo = this.Photo,
                        Type = _userInfo.Type,
                        Password = _userInfo.Password
                    };
                    if ((ErrorVisible))
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Please make sure all information is valid before saving changes.", "OK");
                        return;
                    }
                    if (!HasFieldsChanged(_userInfo))
                    {
                        return;
                    }

                    await userServices.EditProfile(user);
                    if (Photo != _userInfo.Photo)
                    {
                        await userServices.UploadPhoto(Photo, _userInfo.Id);
                    }
                    await _navigation.PopAsync();
                    await PopupNavigation.Instance.PushAsync(new PopUp("The dish has been added successfully"));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (ConnectionException)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was a connection error. Please check your internet connection and try again.", "OK");
            }
            catch (HttpRequestException)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was an HTTP request error. Please try again later.", "OK");

            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred. Please try again later.", "OK");
            }
        }

        public bool HasFieldsChanged(User user)
        {
            return (_email != user.Email) || (_phone != user.Phone) || (_name != user.Name) || (_location != user.Location || _photo != user.Photo);
        }


        private async Task OnCancelClicked()
        {
            string email;
            try
            {
                email = Preferences.Get("userEmail", null);
                if (email is null)
                {
                    return;
                }
                await _navigation.PopAsync();
            }
            catch (ConnectionException)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was a connection error. Please check your internet connection and try again.", "OK");
            }
            catch (HttpRequestException)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was an HTTP request error. Please try again later.", "OK");

            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred. Please try again later.", "OK");
            }
        }
    }
}