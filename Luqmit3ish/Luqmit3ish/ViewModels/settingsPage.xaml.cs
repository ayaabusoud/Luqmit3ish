using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
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
        public INavigation Navigation { get; set; }
        public ICommand MyProfileCommand { protected set; get; }
 
        public ICommand ResetPassCommand { protected set; get; }
        public ICommand LogOutCommand { protected set; get; }


        public UserServices userServices;

        public const string DEFULY_IMAGE = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";


        public UserSettingsViewModel(INavigation navigation) {
            this.Navigation = navigation;

               MyProfileCommand= new Command(async () => await OnProfileClicked());

                ResetPassCommand = new Command(async () => await OnResetClicked());
                LogOutCommand= new Command(async () => await OnLogOutClicked());
              userServices = new UserServices();
            OnInit();
        }

        private async Task OnLogOutClicked()
        {
            await Navigation.PushModalAsync(new LoginPage());

        }

        private async  Task OnResetClicked()
        {
            await Navigation.PushModalAsync(new ResetPassSettingsPage());

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
            Photo = DEFULY_IMAGE;

            try
            {
              string email = Preferences.Get("userEmail", "none");

                UserInfo = await userServices.GetUserByEmail(email);

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

                    Name = UserInfo.Name;
                }
            }
            catch (Exception)
            {
                throw new Exception("Get User Filed");
            }
         ;
        }
        private async Task OnProfileClicked()
        {
            await Navigation.PushModalAsync(new ProfilePage());   
        }
    }
}
