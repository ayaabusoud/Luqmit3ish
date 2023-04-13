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
using Luqmit3ish.Exceptions;
using System.Net.Http;

namespace Luqmit3ish.ViewModels
{
    class RestaurantHomeViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand AddCommand { protected set; get; }
        public Command<int> EditCommand { protected set; get; }
        public Command<int> DeleteCommand { protected set; get; }
        public ICommand NameTapCommand { protected set; get; }
        private FoodServices _foodServices;

        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        public RestaurantHomeViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            AddCommand = new Command(async () => await OnAddClicked());
            EditCommand = new Command<int>(async (int id) => await OnEditClicked(id));
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            NameTapCommand = new Command(async () => await OnTapClicked());
            _foodServices = new FoodServices();
            OnInit();
        }

        private async void OnInit()
        {
            var id = Preferences.Get("userId", null);
            if (id is null)
            {
                return;
            }
            var userId = int.Parse(id);
            try
            {
                Dishes = await _foodServices.GetFoodByResId(userId);
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
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
            await _navigation.PushAsync(new AddFoodPage());

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
            await _navigation.PushAsync(new EditFoodPage(id));

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
                await _foodServices.DeleteFood(id);
                OnInit();
            }
        }
        private async Task OnTapClicked()
        {
            try
            {
            await _navigation.PushAsync(new FoodDetailPage());
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
