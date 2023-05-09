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
                    OnPropertyChanged(nameof(_userInfo.Photo));
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
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                NotAuthorized();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }


        private bool ValidatePhone(string value)
        {
            if (!Regex.IsMatch(value, "^[0-9]{8,15}$"))
            {
                return false;
            }
            return true;
        }
  

        private async Task OnDoneClicked()
        {

            try
            {
                if(string.IsNullOrEmpty(_userInfo.Location) || string.IsNullOrEmpty(_userInfo.Name) ||
                    string.IsNullOrEmpty(_userInfo.OpeningHours) || string.IsNullOrEmpty(_userInfo.Phone)){
                    await PopupNavigation.Instance.PushAsync(new PopUp("Please fill all the information before saving the changes."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }else if(!ValidatePhone(_userInfo.Phone)){
                    await PopupNavigation.Instance.PushAsync(new PopUp("Phone number is invalid"));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }
               
                await userServices.EditProfile(_userInfo);
                await userServices.UploadPhoto(_userInfo.Photo, _userInfo.Id);
                await _navigation.PopAsync();
                await PopupNavigation.Instance.PushAsync(new PopUp("The Profile has been edited successfully"));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
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
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                NotAuthorized();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
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
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
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
