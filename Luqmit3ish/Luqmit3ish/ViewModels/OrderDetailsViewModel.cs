using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class OrderDetailsViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        private OrderCard _order;
        public OrderCard Order
        {
            get => _order;
            set 
            {
                SetProperty(ref _order, value);

                OnPropertyChanged(nameof(Order));

            }
    }


        private ObservableCollection<OrderDish> _items;
        public ObservableCollection<OrderDish> Items
        {
            get => _items;
            set =>  SetProperty(ref _items, value);
        }
        private OrderService _orderService;
        private FoodServices _foodService;
        public ICommand PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public ICommand ProfileCommand { protected set; get; }


        public OrderDetailsViewModel(OrderCard order, INavigation navigation)
        {
            this._navigation = navigation;
            this._order = order;
            this.Items = order.Orders;
            PlusCommand = new Command<OrderDish>(async (OrderDish orderDish) => await OnPlusClicked(orderDish));
            MinusCommand = new Command<OrderDish>(async (OrderDish orderDish) => await OnMinusClickedAsync(orderDish));
            ProfileCommand = new Command<User>(async (User restaurant) => await OnProfileClicked(restaurant));
            _orderService = new OrderService();
            _foodService = new FoodServices();
        }
        private async void getData(int orderId)
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
                    if (order.Id == orderId)
                    {
                        Items = new ObservableCollection<OrderDish>(order.Orders);
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



        private async Task OnMinusClickedAsync(OrderDish orderDish)
        {
            try
            {
                await _orderService.UpdateOrderDishCount(orderDish.Id, "Minus");
                getData(Order.Id);

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
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async Task OnPlusClicked(OrderDish orderDish)
        {
            try
            {
                await _orderService.UpdateOrderDishCount(orderDish.Id, "plus");
                getData(Order.Id);
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
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }
        private async Task OnProfileClicked(User restaurant)
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new OtherProfilePage(restaurant));

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
