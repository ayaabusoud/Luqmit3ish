using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;
using Luqmit3ish.Services;
using System.Collections.ObjectModel;
using Luqmit3ish.Models;

namespace Luqmit3ish.ViewModels
{
    class RestaurantHomeViewModel : ViewModelBase
    {
        public INavigation Navigation { get; set; }
        public ICommand AddCommand { protected set; get; }
        public ICommand EditCommand { protected set; get; }
        public ICommand NameTapCommand { protected set; get; }
        public FoodServices foodServices;

        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        public RestaurantHomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            AddCommand = new Command(async () => await OnAddClicked());
            EditCommand = new Command(async () => await OnEditClicked());
            NameTapCommand = new Command(async () => await OnTapClicked());
            foodServices = new FoodServices();
            OnInit();
        }

        private async void OnInit()
        {
            string id = Preferences.Get("userId", "0");
            int userId = int.Parse(id);
            Dishes = await foodServices.GetFoodByResId(userId);
            foreach(Dish dish in Dishes)
            {
                if(dish.photo == "")
                {
                    dish.photo = "https://st.depositphotos.com/1059926/2823/i/950/depositphotos_28233673-stock-photo-vegetables-on-a-white-background.jpg";
                }
            }
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
