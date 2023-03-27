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
    class CharityHomeViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand FilterCommand { protected set; get; }
        public ICommand SearchCommand { protected set; get; }
        public ICommand ProfileCommand { protected set; get; }
        public ICommand FoodDetailCommand { protected set; get; }
        public CharityHomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            FilterCommand = new Command(async () => await OnFilterClicked());
            SearchCommand = new Command(async () => await OnSearchClicked());
            ProfileCommand = new Command(async () => await OnProfileClicked());
            FoodDetailCommand = new Command(async () => await OnFoodDetailClicked());
        }
        private async Task OnFilterClicked()
        {
            await Navigation.PushAsync(new FilterFoodPage());
        }
        private async Task OnSearchClicked()
        {
            await Navigation.PushAsync(new SearchPage());
        }
        private async Task OnProfileClicked()
        {
            await Navigation.PushAsync(new OtherProfilePage());
        }
        private async Task OnFoodDetailClicked()
        {
            await Navigation.PushAsync(new FoodDetailPage());
        }
    }
}


