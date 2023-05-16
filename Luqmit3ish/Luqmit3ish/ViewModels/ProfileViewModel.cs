using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
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
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand DoneCommand { protected set; get; }
        public ICommand CancelCommand { protected set; get; }
        public ICommand EditPhotoClicked { protected set; get; }
        public IUserServices userServices;

        private User _userInfo;
        public User UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }

        public ProfileViewModel(INavigation navigation, User userInfo)
        {
            this._navigation = navigation;
            _userInfo = userInfo;
            DoneCommand = new Command(async () => await OnDoneClicked());
            CancelCommand = new Command(async () => await OnCancelClicked());
            EditPhotoClicked = new Command(async () => await OnEditPhotoClicked());
            userServices = new UserServices();
        }

        #region Phone Validation
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
                    _phoneDivider = "Gray";
                }
                else
                {
                    _phoneInvalid = true;
                    _phoneDivider = "DarkRed";
                }
                OnPropertyChanged(nameof(PhoneInvalid));
                OnPropertyChanged(nameof(PhoneDivider));
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

        private string _phoneDivider = "Gray";
        public string PhoneDivider
        {
            get => _phoneDivider;
            set
            {
                SetProperty(ref _phoneDivider, value);
            }
        }


        #endregion

        #region Name Validation
        private bool _isNameValid;
        public bool IsNameValid
        {
            get => _isNameValid;
            set
            {

                SetProperty(ref _isNameValid, value);
                if (IsNameValid)
                {
                    _nameInvalid = false;
                    _nameDivider = "Gray";
                }
                else
                {
                    _nameInvalid = true;
                    _nameDivider = "DarkRed";
                }
                OnPropertyChanged(nameof(NameInvalid));
                OnPropertyChanged(nameof(NameDivider));

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

        private string _nameDivider = "Gray";
        public string NameDivider
        {
            get => _nameDivider;
            set
            {
                SetProperty(ref _nameDivider, value);
            }
        }

        #endregion

        #region     Opening Hours Validation
        private bool _isOpeningHoursValid;
        public bool IsOpeningHoursValid
        {
            get => _isOpeningHoursValid;
            set
            {

                SetProperty(ref _isOpeningHoursValid, value);
                if (IsOpeningHoursValid)
                {
                    _openingHoursInvalid = false;
                    _openingHoursDivider = "Gray";
                }
                else
                {
                    _openingHoursInvalid = true;
                    _openingHoursDivider = "DarkRed";
                }
                OnPropertyChanged(nameof(OpeningHoursInvalid));
                OnPropertyChanged(nameof(OpeningHoursDivider));
            }
        }
        private bool _openingHoursInvalid = false;
        public bool OpeningHoursInvalid
        {
            get => _openingHoursInvalid;
            set
            {
                SetProperty(ref _openingHoursInvalid, value);

            }
        }
        private string _openingHoursErrorMessage = "Please enter a valid Opening Hours\n(ex: 11:00am-11:00pm)";
        public string OpeningHoursErrorMessage
        {
            get => _openingHoursErrorMessage;
            set
            {
                SetProperty(ref _openingHoursErrorMessage, value);
            }

        }

        private string _openingHoursDivider = "Gray";
        public string OpeningHoursDivider
        {
            get => _openingHoursDivider;
            set
            {
                SetProperty(ref _openingHoursDivider, value);
            }
        }

        #endregion

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
                    _userInfo.Photo = photoPath;
                    OnPropertyChanged(nameof(UserInfo));
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
  
        private async Task<bool> ValidateFields()
        {
            if (string.IsNullOrEmpty(_userInfo.Location) || string.IsNullOrEmpty(_userInfo.Name) ||
                    string.IsNullOrEmpty(_userInfo.OpeningHours) || string.IsNullOrEmpty(_userInfo.Phone))
            {
                await PopNavigationAsync("Please fill all the information before saving the changes.");
                return false;
            }
            if (_phoneInvalid || _nameInvalid || _openingHoursInvalid)
            {
                await PopNavigationAsync("Please enter a valid fields before save changes.");
                return false;
            }
            return true;
        }

        private async Task OnDoneClicked()
        {

            try
            {
                if (await ValidateFields())
                {
                    await userServices.EditProfile(_userInfo);
                    await userServices.UploadPhoto(_userInfo.Photo, _userInfo.Id);
                    await _navigation.PopAsync();
                    await PopNavigationAsync("The Profile has been edited successfully");
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

      


        private async Task OnCancelClicked()
        {
            try
            {
                await _navigation.PopAsync();
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
 
            }
        }
    }
}
