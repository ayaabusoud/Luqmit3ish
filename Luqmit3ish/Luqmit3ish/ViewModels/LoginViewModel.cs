using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ForgotPassCommand { protected set; get; }
        public ICommand SignupCommand { protected set; get; }
        public Command LoginCommand { protected set; get; }
        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            ForgotPassCommand = new Command(async () => await OnForgotPassClicked());
            SignupCommand = new Command(async () => await OnSignupClicked());
            LoginCommand = new Command(OnLoginClicked);
        }

        private async Task OnForgotPassClicked()
        {
            await Navigation.PushModalAsync(new NavigationPage(new ForgotPasswordPage()));
        }
        private async Task OnSignupClicked()
        {
            await Navigation.PushModalAsync(new SignupPage());
        }

        private void OnLoginClicked(object obj)
        {
            Application.Current.MainPage = new AppShell();
        }
    }
}

