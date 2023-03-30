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
    class EditProfileViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand DoneCommand { protected set; get; }

        public EditProfileViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            DoneCommand = new Command(async () => await OnDoneClicked());
        }
        private async Task OnDoneClicked()
        {
            try
            {
            await Navigation.PopAsync();

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

