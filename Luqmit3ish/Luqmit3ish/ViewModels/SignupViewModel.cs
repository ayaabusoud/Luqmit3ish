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
    class SignupViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SignupCommand { protected set; get; }
        public ICommand LoginCommand { protected set; get; }

        public SignupViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            SignupCommand = new Command(async () => await OnSignupClicked());
            LoginCommand = new Command(async () => await OnLoginClicked());
        }

        private async Task OnSignupClicked()
        {
            await Navigation.PushModalAsync(new VerificationPage());
        }
        private async Task OnLoginClicked()
        {
            Navigation.PushModalAsync(new LoginPage());
        }
    }
}
