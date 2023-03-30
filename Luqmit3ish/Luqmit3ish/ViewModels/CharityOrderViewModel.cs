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
    class CharityOrderViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand EditCommand { protected set; get; }
        public ICommand ProfileCommand { protected set; get; }
        public CharityOrderViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            EditCommand = new Command(async () => await OnEditClicked());
            ProfileCommand = new Command(async () => await OnProfileClicked());
        }
        private async Task OnEditClicked()
        {
            try
            {
            await Navigation.PushAsync(new EditOrderPage());

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
        private async Task OnProfileClicked()
        {
            try
            {
                await Navigation.PushAsync(new OtherProfilePage());

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
