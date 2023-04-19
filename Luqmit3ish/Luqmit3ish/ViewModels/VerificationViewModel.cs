using Luqmit3ish.Models;
using Luqmit3ish.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class VerificationViewModel  : ViewModelBase
    {
        private readonly EmailService _emaiService;
        private readonly UserServices _userServices;
        private string sentCode;
        public ICommand ContinueCommand { get; }

        public ICommand ResendCommand { get; }
        public VerificationViewModel(SignUpRequest signUpRequest)
        {
            _emaiService = new EmailService();
            _userServices = new UserServices();
            ContinueCommand = new Command(async () => await OnContinueClicked(signUpRequest));
            ResendCommand = new Command(async () => await OnResendClicked(signUpRequest.Name, signUpRequest.Email));
            OnInit(signUpRequest.Name, signUpRequest.Email);
        }

        private int _pin;
        public int PIN
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        private async Task OnResendClicked(string recipientName, string recipientEmail)
        {
            OnInit(recipientName, recipientEmail);
            await Application.Current.MainPage.DisplayAlert("Successfully", "We have been resent the verfication code", "ok");
            
        }

        private async Task OnContinueClicked(SignUpRequest newUser)
        {
            try
            {
                var code = int.Parse(sentCode);
                if(code == PIN)
                {
                    bool IsInserted = await _userServices.InsertUser(newUser);
                    if (IsInserted)
                    {
                        User user = await _userServices.GetUserByEmail(newUser.Email);

                        if (user.Type.Equals("Restaurant"))
                        {
                            Application.Current.MainPage = new AppShellRestaurant();
                        }
                        else
                        {
                            Application.Current.MainPage = new AppShellCharity();
                        }
                    }
                }
                else
                {
                    return;
                }
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
        private async void OnInit(string recipientName, string recipientEmail)
        {
            
             sentCode = await _emaiService.SendVerificationCode(recipientName, recipientEmail);
        }
    }
}
