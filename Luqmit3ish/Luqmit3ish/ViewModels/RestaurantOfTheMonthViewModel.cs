using Luqmit3ish.Exceptions;
using Luqmit3ish.Services;
using Luqmit3ish.ViewModels;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Luqmit3ish.Interfaces;

namespace Luqmit3ish.Views
{
    class RestaurantOfTheMonthViewModel : ViewModelBase
    {
        private int _dishes;
        private IOrderService _orderService;
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
        private bool _emptyResult;

        public bool EmptyResult
        {
            get => _emptyResult;
            set => SetProperty(ref _emptyResult, value);
        }
        private bool _nonEmptyResult;

        public bool NonEmptyResult
        {
            get => _nonEmptyResult;
            set => SetProperty(ref _nonEmptyResult, value);
        }
        

        public RestaurantOfTheMonthViewModel()
        {
            _orderService = new OrderService();
            OnInit();
        }
        private void OnInit()
        {
            try
            {



            
            Task.Run(async () => {
                try
                {
                    var bestRestaurant = await _orderService.GetBestRestaurant();
                    if (bestRestaurant != null)
                    {
                        _dishes = bestRestaurant.Dishes;
                        _restaurantName = bestRestaurant.RestaurantName;
                    }
                    if (Dishes == 0)
                    {
                        _emptyResult = true;
                        _nonEmptyResult = false;
                    }
                    else
                    {
                        _emptyResult = false;
                        _nonEmptyResult = true;
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
            }).Wait();
           }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
           

        }
    }
}
