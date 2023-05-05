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
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class VerificationViewModel  : ViewModelBase
    {
        private readonly IEmailService _emaiService;
        private readonly IUserServices _userServices;
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

        private string _pin = null;
        public string PIN
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        private async Task OnResendClicked(string recipientName, string recipientEmail)
        {

            OnInit(recipientName, recipientEmail);
            await PopupNavigation.Instance.PushAsync(new PopUp("We have been resent the verfication code."));
            Thread.Sleep(3000);
            await PopupNavigation.Instance.PopAsync();            
        }

        private async Task OnContinueClicked(SignUpRequest newUser)
        {
            try
            {
                var code = int.Parse(sentCode);

                int enteredCode = int.Parse(_pin);

                Debug.WriteLine(code);
                Debug.WriteLine(_pin);

                if (code == enteredCode)
                {


                    bool IsInserted = await _userServices.InsertUser(newUser);

                    if (IsInserted)
                    {
                        if (newUser.Type.Equals("Restaurant"))
                        {
                            Application.Current.MainPage = new AppShellRestaurant();
                        }
                        else
                        {
                            Application.Current.MainPage = new AppShellCharity();
                        }
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new PopUp("The code is incorrect, please try again."));
                        Thread.Sleep(3000);
                        await PopupNavigation.Instance.PopAsync();
                        return;
                    }
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
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
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }
        private void OnInit(string recipientName, string recipientEmail)
        {
            try
            {

            
            Task.Run(async () => {
                try
                {
                    sentCode = await _emaiService.SendVerificationCode(recipientName, recipientEmail);

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
            }).Wait();
           }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
