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
        public Command<int> EditCommand { protected set; get; }
        public Command<int> DeleteCommand { protected set; get; }
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
            EditCommand = new Command<int>(async (int id) => await OnEditClicked(id));
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            NameTapCommand = new Command(async () => await OnTapClicked());
            foodServices = new FoodServices();
            OnInit();
        }

        private async void OnInit()
        {
            string id = Preferences.Get("userId", "0");
            int userId = int.Parse(id);
            Dishes = await foodServices.GetFoodByResId(userId);

            if (Dishes != null)
            {
                foreach (Dish dish in Dishes)
                {
                    if (dish.number == 0)
                    {
                        Dishes.Remove(dish);
                    }
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
        private async Task OnEditClicked(int id)
        {
            try
            {
            await Navigation.PushAsync(new EditFoodPage(id));

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
        private async Task OnDeleteClicked(int id)
        {
            var deleteConfirm = await Application.Current.MainPage.DisplayAlert("", "Are you sure that you want to delete this dish?", "Yes", "No");
            if (deleteConfirm)
            {
                await foodServices.DeleteFood(id);
                OnInit();
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
