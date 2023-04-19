using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class OnBoardingViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand SignupCommand { protected set; get; }

        public OnBoardingViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            SignupCommand = new Command(async () => await OnButtonClicked());
        }

        private async Task OnButtonClicked()
        {
            try
            {
                await _navigation.PushModalAsync(new SignupPage());

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