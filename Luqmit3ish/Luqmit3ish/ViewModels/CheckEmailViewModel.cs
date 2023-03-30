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
    class CheckEmailViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ButtonCommand { protected set; get; }

        public CheckEmailViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            ButtonCommand = new Command(async () => await OnButtonClicked());
        }
        private async Task OnButtonClicked()
        {
            try
            {
            await Navigation.PushModalAsync(new ResetPasswordPage());

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
