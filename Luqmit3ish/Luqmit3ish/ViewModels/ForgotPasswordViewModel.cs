using Luqmit3ish.Interfaces;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
   
    class ForgotPasswordViewModel :ViewModelBase
    {
        public ICommand SendEmailCommand { protected set; get; }
        public ICommand LoginCommand { protected set; get; }
        private IUserServices _userService;

        public ForgotPasswordViewModel()
        {
            SendEmailCommand = new Command(async () => await OnSendEmailClicked());
            LoginCommand = new Command(OnLoginClicked);
            _userService = new UserServices();
        }
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private async Task OnSendEmailClicked()
        {
            try
            {
                var user = await _userService.GetUserByEmail(Email);
                if(user == null)
                {
                    await PopNavigationAsync("The email you have entered is incorrect.");
                    return;
                }
                Application.Current.MainPage = new CheckEmailPage(Email);

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
        private void OnLoginClicked()
        {
            try
            {
                Application.Current.MainPage = new LoginPage();
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