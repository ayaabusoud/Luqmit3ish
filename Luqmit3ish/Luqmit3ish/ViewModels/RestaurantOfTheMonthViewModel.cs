using Luqmit3ish.Exceptions;
using Luqmit3ish.Services;
using Luqmit3ish.ViewModels;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;

namespace Luqmit3ish.Views
{
    class RestaurantOfTheMonthViewModel : ViewModelBase
    {
        private int _dishes;
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


        public RestaurantOfTheMonthViewModel(DishesOrder bestRestaurant)
        {
            OnInit(bestRestaurant);
        }
        private void OnInit(DishesOrder bestRestaurant)
        {
            try
            {



            
            Task.Run(async () => {
                try
                {
                   
                    if (bestRestaurant != null)
                    {
                        _dishes = bestRestaurant.Dishes;
                        _restaurantName = bestRestaurant.RestaurantName;
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
