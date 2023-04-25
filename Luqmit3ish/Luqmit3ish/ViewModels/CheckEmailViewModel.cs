using Luqmit3ish.Exceptions;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class CheckEmailViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        private string verificationCode;

        private EmailService _emailService;
        public ICommand ResetCommand { protected set; get; }
        public CheckEmailViewModel(INavigation navigation, string Email)
        {
            this._navigation = navigation;
            ResetCommand = new Command(() => OnContinueClicked(Email));
            _emailService = new EmailService();
            OnInit(Email);
        }

        private int _pin;
        public int PIN
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        private void OnInit(string Email)
        {
            Task.Run(async () => {
                try
                {
                    verificationCode = await _emailService.SendVerificationCode("", Email);

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

        private void OnContinueClicked(string Email)
        {
            try
            {
                var code =  int.Parse(verificationCode);
                if(code == PIN)
                {
                    Application.Current.MainPage = new ResetPasswordForgetPage(Email);
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

    }

}
