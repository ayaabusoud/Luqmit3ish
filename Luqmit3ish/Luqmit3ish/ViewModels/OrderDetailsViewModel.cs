using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace Luqmit3ish.ViewModels
{
    class OrderDetailsViewModel : ViewModelBase
    {
        private User _user;
        private OrderCard _order;
        private UserServices _userServices;
        private OrderService _orderService;
        private FoodServices _foodService;
        public Command<int> PlusCommand { protected set; get; }
        public Command<int> MinusCommand { protected set; get; }

        public OrderDetailsViewModel(OrderCard order)
        {
            this._order = order;
            RestaurantName = _order.name;
            UserPhoto = _order.image;
            OnPropertyChanged(nameof(Dishes));
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command<int>(OnMinusClicked);
            _userServices = new UserServices();
            _orderService = new OrderService();
            _foodService = new FoodServices();
            Id = order.id;
            Initial(Id);

        }
        private ObservableCollection<OrderDish> _dishes;

        public ObservableCollection<OrderDish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        private int _id;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        private string _userPhoto;
        public string UserPhoto
        {
            get => _userPhoto;
            set => SetProperty(ref _userPhoto, value);
        }


        private async void Initial(int id)
        {

            var user = Preferences.Get("userId", null);
            if (user is null)
            {
                return;
            }
            var userId = int.Parse(user);
            try
            {
                ObservableCollection<OrderCard> cards = await _orderService.GetOrders(userId);
                foreach (OrderCard order in cards)
                {
                    if (order.id == id)
                    {
                        Dishes = new ObservableCollection<OrderDish>(order.data);
                    }
                }
        
            }
            catch (ConnectionException e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was a connection error. Please check your internet connection and try again.", "OK");
            }
            catch (HttpRequestException e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was an HTTP request error. Please try again later.", "OK");

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred. Please try again later.", "OK");
            }

        }


        private string _restaurantName;
        public string RestaurantName
        {
            get => _restaurantName;
            set => SetProperty(ref _restaurantName, value);
        }

        public OrderCard Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }
        private async void OnMinusClicked(int orderId)
        {
            await _orderService.UpdateOrderDishCount(orderId, "Minus");
            Initial(Id);
        }

        private async void OnPlusClicked(int orderId)
        {
            Debug.WriteLine(orderId);
            try
            {
                var order = await _orderService.GetOrderById(orderId);
                if (order != null)
                {
                    Dish dish = await _foodService.GetFoodById(order.dish_id);
                    if (dish == null)
                    {
                        return;
                    }
                    if (dish.number == 0)
                    {

                        return;
                    }
                }
                await _orderService.UpdateOrderDishCount(orderId, "plus");
                Initial(Id);
            }
            catch (ConnectionException e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was a connection error. Please check your internet connection and try again.", "OK");
            }
            catch (HttpRequestException e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was an HTTP request error. Please try again later.", "OK");

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred. Please try again later.", "OK");
            }
        }
    }

}
