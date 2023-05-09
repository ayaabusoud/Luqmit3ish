using System;
using Luqmit3ish.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
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
        private readonly IOrderService _orderService;
        public ICommand PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public ICommand ProfileCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }

        public OrderDetailsViewModel(OrderCard order, INavigation navigation)
        {
            this._navigation = navigation;
            this._order = order;
            this.Items = order.Orders;
            PlusCommand = new Command<OrderDish>(async (OrderDish orderDish) => await OnPlusClicked(orderDish));
            MinusCommand = new Command<OrderDish>(async (OrderDish orderDish) => await OnMinusClickedAsync(orderDish));
            ProfileCommand = new Command<User>(async (User restaurant) => await OnProfileClicked(restaurant));
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            _orderService = new OrderService();
        }
        private async void GetData(int orderId)
        {
            try
            {
                var userId = GetUserId();

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

        }

        private async Task OnDeleteClicked(int restaurantId)
        {
            var deleteConfirm = await Application.Current.MainPage.DisplayAlert("Delete Order",
                "Are you sure that you want to delete this Order?", "Yes", "No");
            if (deleteConfirm)
            {
                var userId = GetUserId();
                try
                {
                    bool result = await _orderService.DeleteOrder(userId, restaurantId);
                    if (result == true)
                    {
                        await _navigation.PopAsync();
                        await PopNavigationAsync("The order have been deleted successfully.");
                    }
                    else
                    {
                        await PopNavigationAsync("The Order has not been deleted, please try again.");
                    }
                }
                catch (ConnectionException e)
                {
                    Debug.WriteLine(e.Message);
                    await PopNavigationAsync(InternetMessage);
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine(e.Message);
                    await PopNavigationAsync(HttpRequestMessage);
                }
                catch (NotAuthorizedException e)
                {
                    Debug.WriteLine(e.Message);
                    NotAuthorized();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    await PopNavigationAsync(ExceptionMessage);
                }
            }

        }

        private async Task OnMinusClickedAsync(OrderDish orderDish)
        {
            try
            {
                await _orderService.UpdateOrderDishCount(orderDish.Id, "Minus");
                GetData(Order.Id);

            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(HttpRequestMessage);
            }
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                NotAuthorized();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task OnPlusClicked(OrderDish orderDish)
        {
            try
            {
                await _orderService.UpdateOrderDishCount(orderDish.Id, "plus");
                GetData(Order.Id);
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(HttpRequestMessage);
            }
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                NotAuthorized();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
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
