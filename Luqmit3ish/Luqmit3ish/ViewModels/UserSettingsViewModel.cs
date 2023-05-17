using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    public class UserSettingsViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand MyProfileCommand { protected set; get; }
        public ICommand ResetPassCommand { protected set; get; }
        public ICommand RestaurantCommand { protected set; get; }
        public ICommand LogOutCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }
        public ICommand DarkModeCommand { protected set; get; }
        private readonly IUserServices _userServices;
        private const String _rongMessage = "Something went wrong, please try again.";
        private const String _deleteSucsessMessage = "The Account have been deleted successfully.";
        private const String _insureDeleteMessage = "Are you sure that you want to delete Your account?"; 
        private const String _deleteAccount = "Delete Account";


        public UserSettingsViewModel(INavigation navigation) {
               this._navigation = navigation;
            MyProfileCommand = new Command(async () => await OnProfileClicked());
            ResetPassCommand = new Command(async () => await OnResetClicked());
               LogOutCommand= new Command(async () => await OnLogOutClicked());
               DeleteCommand = new Command<int>(async (int id) => await OnDeleteAccountClicked(id));
               RestaurantCommand = new Command(async () => await OnRestaurantClicked());
            _darkTheme = Preferences.Get("DarkTheme", false);
            _swichColor = _darkTheme ? Color.DarkOrange : Color.White;
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

        private Color _swichColor;
        public Color SwichColor
        {
            get => _swichColor;
            set => SetProperty(ref _swichColor, value);
        }

        private bool _darkTheme;
        public bool DarkTheme
        {
            get => Preferences.Get("DarkTheme", false);

            set
            {
                if (value)
                {
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                }
                else
                {
                    App.Current.UserAppTheme = OSAppTheme.Light;
                }
                Preferences.Set("DarkTheme", value);
                OnPropertyChanged(nameof(DarkTheme));
                _swichColor = DarkTheme ? Color.DarkOrange : Color.White;
                OnPropertyChanged(nameof(SwichColor));
            }
        }

        private void OnInit()
        {
            try
            {


                Task.Run(async () => { 
                 try
            {
                string email = Preferences.Get("userEmail", "none");
                if (string.IsNullOrEmpty(email)) { 
                    return; 
                }
                UserInfo = await _userServices.GetUserByEmail(email);

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
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        await PopNavigationAsync(ExceptionMessage);
                    }


                }).Wait();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
           

        }

        private async Task OnDeleteAccountClicked(int id)
        {
            var deleteConfirm = await Application.Current.MainPage.DisplayAlert(_deleteAccount, _insureDeleteMessage , "Yes", "No");
            if (deleteConfirm)
            {
                try
                {
                    bool result = await _userServices.DeleteAccount(id); 
                    if (result)
                    {
                        Preferences.Clear();
                        Application.Current.MainPage = new LoginPage();
                        await PopupNavigation.Instance.PushAsync(new PopUp(_deleteSucsessMessage));
                        Thread.Sleep(3000);
                        await PopupNavigation.Instance.PopAsync();

                    }
                    else
                    {
                        await PopNavigationAsync(_rongMessage);
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
        }

        private async Task OnLogOutClicked()
        {
            try
            {
                RemovePreferences();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                await _navigation.PopToRootAsync();
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

        private void RemovePreferences()
        {
            Preferences.Remove("Token");
            Preferences.Remove("userEmail");
            Preferences.Remove("userId");
        }

        private async Task OnResetClicked()
        {
            try
            {
                await _navigation.PushAsync(new ResetPasswordPage(UserInfo.Email));
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

        private async Task OnProfileClicked()
        {
            try
            {
                await _navigation.PushAsync(new ProfilePage(UserInfo));
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

    }
}
