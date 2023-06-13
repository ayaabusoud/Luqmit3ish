using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class CheckEmailViewModel : ViewModelBase
    {

        private string verificationCode;

        private IEmailService _emailService;
        public ICommand ResetCommand { protected set; get; }
        public CheckEmailViewModel(string Email)
        {
            ResetCommand = new Command(() => OnContinueClicked(Email));
            _emailService = new EmailService();
            OnInit(Email);
        }

        private string _pin;
        public string PIN
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        private void OnInit(string Email)
        {
            try
            {

            Task.Run(async () => {
                try
                {
                    verificationCode = await _emailService.SendVerificationCode(Email, Email);

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

        private void OnContinueClicked(string Email)
        {
            try
            {
                var code = int.Parse(verificationCode);
                Console.WriteLine(code);
                if (code == int.Parse(PIN))
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
