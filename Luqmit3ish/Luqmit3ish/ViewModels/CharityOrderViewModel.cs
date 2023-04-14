using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PancakeView;

namespace Luqmit3ish.ViewModels
{
    class CharityOrderViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand EditCommand { protected set; get; }
        public ICommand Search { protected set; get; }

        public ICommand ProfileCommand { protected set; get; }
        private OrderService _orderService;
        private FoodServices _foodService;

        public ICommand DeleteCommand { protected set; get; }
        public Command<int> PlusCommand { protected set; get; }
        public Command<int> MinusCommand { protected set; get; }


        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        public CharityOrderViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            EditCommand = new Command<int>(async (int id) => await OnEditClicked(id));
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            Search = new Command(async () => await OnSearchClicked());
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command<int>(OnMinusClicked);
            ExpanderCommand = new Command<int>(OnExpanderClicked);
            _orderService = new OrderService();
            _foodService = new FoodServices();
            OnInit();
        }


        private async Task OnSearchClicked()
        {
            await _navigation.PushAsync(new SearchPage());
        }



        private bool _isExpanded = false;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }
        public Command<int> ExpanderCommand { protected set; get; }
        private void OnExpanderClicked(int id)
        {
            var item = OrderCard.FirstOrDefault(i => i.id == id);
            if (item != null)
            {
                if (item.IsExpanded)
                {
                    item.IsExpanded = false;
                }
                else
                {
                    item.IsExpanded = true;
                }
            }

        }
        private async void OnMinusClicked(int orderId)
        {
            Order order;
            Dish dish;
            const string MinusString = "Minus";

            var id = Preferences.Get("userId", null);
            if (id == null)
            {
                return;
            }
            var userId = int.Parse(id);
            try
            {
                await _orderService.UpdateOrderDishCount(orderId,MinusString );
                order = await _orderService.GetOrderById(orderId);
                dish = await _foodService.GetFoodById(order.dish_id);
                OrderCard = await _orderService.GetOrders(userId);
                if (OrderCard == null || order == null || dish == null)
                {
                    return;
                }
                foreach (OrderCard item in OrderCard)
                {
                    if (item.id == dish.user_id)
                    {
                        item.IsExpanded = true;
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

        private async void OnPlusClicked(int orderId)
        {
          
            Dish dish;
            Order order;
            const string PlusString = "plus";

            var id = Preferences.Get("userId", null);
            if (id == null)
            {
                return;
            }
            var userId = int.Parse(id);
            try
            {
                order = await _orderService.GetOrderById(orderId);
                dish = await _foodService.GetFoodById(order.dish_id);
                OrderCard = await _orderService.GetOrders(userId);
                if (dish.number == 0)
                {
                    return;
                }
                if (OrderCard == null || order == null || dish == null)
                {
                    return;
                }
                await _orderService.UpdateOrderDishCount(orderId,PlusString );            
                foreach (OrderCard item in OrderCard)
                {
                    if (item.id == dish.user_id)
                    {
                        item.IsExpanded = true;
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

private async Task OnDeleteClicked(int restaurantId)
        {
            var deleteConfirm = await Application.Current.MainPage.DisplayAlert("Delete Order", "Are you sure that you want to delete this Order?", "Yes", "No");
            if (deleteConfirm)
            {
                var id = Preferences.Get("userId", null);
                if(id is null)
                {
                    return;
                }
                var userId = int.Parse(id);
                try
                {
                    bool result =  await _orderService.DeleteOrder(userId, restaurantId);
                    if(result == true)
                    {
                    await App.Current.MainPage.DisplayAlert("Success", "The order have been deleted successfully", "ok");
                    OnInit();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Faild", "The Order has not been deleted , please try again", "ok");
                    }
                }
                catch (Exception e)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "An error occur, please try again", "ok");

                }
            }

        }

        private ObservableCollection<OrderCard> _orderCard;

        public ObservableCollection<OrderCard> OrderCard
        {
            get => _orderCard;
            set => SetProperty(ref _orderCard, value);
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
                OrderCard = await _orderService.GetOrders(userId);
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


        private async Task OnEditClicked(int Restaurantid)
        {
            try
            {
            await _navigation.PushAsync(new EditOrderPage());

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
        private async Task OnProfileClicked()
        {

            try
            {
                await _navigation.PushAsync(new OtherProfilePage(1));

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
