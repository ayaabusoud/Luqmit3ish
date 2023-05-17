using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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
            await PopNavigationAsync("We have resent the verfication code.");
        }

        private async Task OnContinueClicked(SignUpRequest newUser)
        {
            try
            {
                var code = int.Parse(sentCode);

                int enteredCode = int.Parse(_pin);

                if (code == enteredCode)
                {
                    if (!Object.ReferenceEquals(newUser, null))
                    {
                        bool IsInserted = await _userServices.InsertUser(newUser);

                        string token = Preferences.Get("Token", string.Empty);
                        string userId = string.Empty;
                        string userEmail = string.Empty;
                        string userType = string.Empty;
                        if (!string.IsNullOrEmpty(token))
                        {
                            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

                            // access the token claims
                            userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                            userEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                            userType = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                        }

                        Preferences.Set("userId", userId);
                        Preferences.Set("userEmail", userEmail);
                        if (IsInserted)
                        {
                            if (userType.Equals("Restaurant"))
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
                            await PopNavigationAsync("The code is incorrect, please try again.");
                            return;
                        }
                    }
                    else
                    {
                        await PopNavigationAsync(ExceptionMessage);
                        return;
                    }
                }

            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
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
    }
}
