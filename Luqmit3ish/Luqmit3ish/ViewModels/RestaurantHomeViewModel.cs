using Luqmit3ish.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using Luqmit3ish.Services;
using System.Collections.ObjectModel;
using Luqmit3ish.Models;
using Luqmit3ish.Exceptions;
using System.Net.Http;
using Luqmit3ish.Interfaces;

namespace Luqmit3ish.ViewModels
{
    class RestaurantHomeViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand AddCommand { protected set; get; }
        public Command<Dish> FoodDetailCommand { protected set; get; }
        private readonly IFoodServices _foodServices;

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
            FoodDetailCommand = new Command<Dish>(async (Dish dish) => await OnFrameClicked(dish));
            _foodServices = new FoodServices();
            OnInit();
        }

        private void OnInit()
        {
            try
            {


            Task.Run(async () => {
                    var userId = GetUserId();
            try
            {
                Dishes = await _foodServices.GetFoodByResId(userId);
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
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
                
                }).Wait();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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

       
    
    }
}
