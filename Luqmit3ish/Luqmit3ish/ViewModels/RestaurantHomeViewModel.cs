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
    class RestaurantHomeViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddCommand { protected set; get; }
        public ICommand EditCommand { protected set; get; }
        public ICommand NameTapCommand { protected set; get; }
        public RestaurantHomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            AddCommand = new Command(async () => await OnAddClicked());
            EditCommand = new Command(async () => await OnEditClicked());
            NameTapCommand = new Command(async () => await OnTapClicked());
        }

        private async Task OnAddClicked()
        {
            try
            {
            await Navigation.PushAsync(new AddFoodPage());

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
        private async Task OnEditClicked()
        {
            try
            {
            await Navigation.PushAsync(new EditFoodPage());

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
        private async Task OnTapClicked()
        {
            try
            {
            await Navigation.PushAsync(new FoodDetailPage());
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
