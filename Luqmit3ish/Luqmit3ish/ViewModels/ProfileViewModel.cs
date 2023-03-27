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
    class ProfileViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand EditCommand { protected set; get; }

        public ProfileViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            EditCommand = new Command(async () => await OnEditClicked());
        }

        private async Task OnEditClicked()
        {
            await Navigation.PushAsync(new EditProfilePage());
        }

    }
}
