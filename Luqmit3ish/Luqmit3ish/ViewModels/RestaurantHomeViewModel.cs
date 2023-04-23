using Luqmit3ish.Views;
using System;
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
using Rg.Plugins.Popup.Services;
using System.Threading;

namespace Luqmit3ish.ViewModels
{
    class RestaurantHomeViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand AddCommand { protected set; get; }
        public Command<int> DeleteCommand { protected set; get; }
        public Command<Dish> FoodDetailCommand { protected set; get; }
        private FoodServices _foodServices;

        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        private bool _emptyResult;

        public bool EmptyResult
        {
            get => _emptyResult;
            set => SetProperty(ref _emptyResult, value);
        }
        public RestaurantHomeViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            AddCommand = new Command(async () => await OnAddClicked());
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            FoodDetailCommand = new Command<Dish>(async (Dish dish) => await OnFrameClicked(dish));
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
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if ( Dishes.Count > 0)
            {
                EmptyResult = false;
                foreach (Dish dish in Dishes)
                {
                    if (dish.Quantity == 0)
                    {
                        Dishes.Remove(dish);
                    }
                }
            }
            else
            {
                EmptyResult = true;
            }
        }
        private async Task OnFrameClicked(Dish dish)
        {
            try
            {
                await _navigation.PushAsync(new EditFoodPage(dish));
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

        private async Task OnDeleteClicked(int id)
        {
            var deleteConfirm = await Application.Current.MainPage.DisplayAlert("", "Are you sure that you want to delete this dish?", "Yes", "No");
            if (deleteConfirm)
            {
                await _foodServices.DeleteFood(id);
                OnInit();
            }
        }
    
    }
}
