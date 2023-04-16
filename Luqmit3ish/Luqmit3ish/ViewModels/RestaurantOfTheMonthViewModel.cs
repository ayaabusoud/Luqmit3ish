
using Luqmit3ish.Exceptions;
using Luqmit3ish.Services;
using Luqmit3ish.ViewModels;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.Views
{
    class RestaurantOfTheMonthViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        private int _dishes;
        private OrderService _orderService;
        public int Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        private string _restaurantName;

        public string RestaurantName
        {
            get => _restaurantName;
            set => SetProperty(ref _restaurantName, value);
        }

        public RestaurantOfTheMonthViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            _orderService = new OrderService();
            OnInit();
        }
        private async Task OnInit()
        {
            try
            {
                var bestRestaurant = await _orderService.GetBestRestaurant();
                if (bestRestaurant != null)
                {
                    Dishes = bestRestaurant.Dishes;
                    RestaurantName = bestRestaurant.RestaurantName;
                }
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
           

        }
    }
}
