using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Services;
using Luqmit3ish.Utilities;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    public class UploadImagePopUpViewModel
    {
        public ICommand CancelCommand { protected set; get; }
        public ICommand CameraCommand { protected set; get; }
        public ICommand GalleryCommand { protected set; get; }

        public UploadImagePopUpViewModel(INavigation navigation)
        {
            CancelCommand = new Command(async () => await OnCancelClickedAsync());
            CameraCommand = new Command(async () => await OnCameraClickedAsync());
            GalleryCommand = new Command(async () => await OnGalleryClickedAsync());
        }

        private async Task OnCancelClickedAsync()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
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

        private async Task OnCameraClickedAsync()
        {
            try
            {
                string photoPath = string.Empty;
                var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take Photo"
                });

                if (result != null)
                {
                    photoPath = result.FullPath;
                }

                MessagingCenter.Send<UploadImagePopUpViewModel, string>(this, "PhotoPath", photoPath);

                await PopupNavigation.Instance.PopAsync();
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

        private async Task OnGalleryClickedAsync()
        {
            try
            {
                string photoPath = string.Empty;
                await Permissions.RequestAsync<Permissions.Photos>();

                var result = await MediaPicker.PickPhotoAsync();

                if (result != null)
                {
                    photoPath = result.FullPath;
                }

                MessagingCenter.Send<UploadImagePopUpViewModel, string>(this, "PhotoPath", photoPath);
                await PopupNavigation.Instance.PopAsync();
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
    }
}