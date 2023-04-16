using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace Luqmit3ish.ViewModels
{
    public class UserSettingsViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand MyProfileCommand { protected set; get; }
        public ICommand ResetPassCommand { protected set; get; }
        public ICommand RestaurantCommand { protected set; get; }
        public ICommand LogOutCommand { protected set; get; }
        public ICommand DeleteAccountCommand { protected set; get; }
        public ICommand DarkModeCommand { protected set; get; }




        public const string DefaultImage = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";

        private readonly UserServices _userServices;


        public UserSettingsViewModel(INavigation navigation) {
               this._navigation = navigation;
               MyProfileCommand= new Command(async () => await OnProfileClicked());
               ResetPassCommand = new Command(async () => await OnResetClicked());
               LogOutCommand= new Command(async () => await OnLogOutClicked());
               DeleteAccountCommand = new Command(async () => await OnDeleteAccountClicked());
               DarkModeCommand = new Command(async () => await OnDarkModeClicked());
               RestaurantCommand = new Command(async () => await OnRestaurantClicked());
            _userServices = new UserServices();
              OnInit();
        }

        private async Task OnRestaurantClicked()
        {
            try
            {
                await _navigation.PushAsync(new RestaurantOfTheMonth());

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

        private User _userInfo;
        public User UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
             
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

        private async void OnInit()
        {

            try
            {
              string email = Preferences.Get("userEmail", "none");
                if (string.IsNullOrEmpty(email)) { 
                    return; 
                }
                UserInfo = await _userServices.GetUserByEmail(email);

                if (UserInfo != null)
                {
                    if (String.IsNullOrEmpty(UserInfo.Photo))
                    {
                        Photo = DefaultImage;

                    }
                    else
                    {
                        Photo = UserInfo.Photo;
                       
                    }

                    Name = UserInfo.Name;
                }
            }
            catch (ConnectionException )
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was a connection error. Please check your internet connection and try again.", "OK");
            }
            catch (HttpRequestException )
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was an HTTP request error. Please try again later.", "OK");

            }
            catch (Exception )
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred. Please try again later.", "OK");
            }

        }
        private async Task OnDarkModeClicked()
        {
            //implm
        }

        private async Task OnDeleteAccountClicked()
        {
            //implem
        }

        private async Task OnLogOutClicked()
        {
            Preferences.Clear();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
            await _navigation.PopToRootAsync();
        }


        private async Task OnResetClicked()
        {
            await _navigation.PushModalAsync(new ResetPassSettingsPage());

        }
        private async Task OnProfileClicked()
        {
            await _navigation.PushModalAsync(new ProfilePage());   
        }
    }
}
