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
    class RestaurantOrderViewModel : ViewModelBase
    {
          private INavigation _navigation { get; set; }

        public ICommand ProfileCommand { protected set; get; }
        public ICommand DoneCommand { protected set; get; }
        public ICommand NotRecievedCommand { protected set; get; }
        public ICommand RecievedCommand { protected set; get; }
        private OrderService _orderService;
        public Command<OrderCard> OrderCommand { protected set; get; }


        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        public RestaurantOrderViewModel(INavigation navigation)
        {
           _navigation = navigation;
            DoneCommand = new Command<OrderCard>(async (OrderCard order) => await OnDoneClick(order));
            NotRecievedCommand = new Command(OnNotRecievedClicked);
            OrderCommand = new Command<OrderCard>(async (OrderCard order) => await OnFrameClicked(order));
            RecievedCommand = new Command(OnRecievedClicked);
            _orderService = new OrderService();
            Selected(false);
        }
        private bool _emptyResult;

        public bool EmptyResult
        {
            get => _emptyResult;
            set => SetProperty(ref _emptyResult, value);
        }
        
        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
        private bool _receievedCheck = true;
        public bool ReceievedCheck
        {
            get => _receievedCheck;
            set => SetProperty(ref _receievedCheck, value);
        }
        private string _recievedColor = "#D9D9D9";
        public string RecievedColor
        {
            get => _recievedColor;
            set => SetProperty(ref _recievedColor, value);
        }
        private string _notRecievedColor = "DarkOrange";
        public string NotRecievedColor
        {
            get => _notRecievedColor;
            set => SetProperty(ref _notRecievedColor, value);
        }
        private void OnRecievedClicked()
        {
            IsVisible = false;
            RecievedColor = "DarkOrange";
            NotRecievedColor = "#D9D9D9";
            Selected(true);
            ReceievedCheck = false;
        }

        private void OnNotRecievedClicked()
        {
            IsVisible = true;
            NotRecievedColor = "DarkOrange";
            RecievedColor = "#D9D9D9";
            Selected(false);
            ReceievedCheck = true;
        }

        private async Task OnFrameClicked(OrderCard order)
        {
            try
            {
                await _navigation.PushAsync(new OrderDetailsPage(order));

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async Task OnDoneClick(OrderCard orders)
        {
            try
            {
                foreach (OrderDish order in orders.data)
                {
                    await _orderService.UpdateOrderReceiveStatus(order.id);
                }
                Selected(false);
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (ConnectionException)
            {
                await Application.Current.MainPage.DisplayAlert("Bad Request", "Please check your connection", "Ok");
            }
            catch (HttpRequestException)
            {
                await Application.Current.MainPage.DisplayAlert("Sorry", "Something went bad here, you can try again", "Ok");
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "ok");
            }
        }


   

        private ObservableCollection<OrderCard> _orderCard;

        public ObservableCollection<OrderCard> OrderCard
        {
            get => _orderCard;
            set => SetProperty(ref _orderCard, value);
        }

        private async void Selected(bool status)
        {
            var id = Preferences.Get("userId", null);
            if (id == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Your login session has been expired", "Ok");
                   await _navigation.PushAsync(new LoginPage());
                    return;
                }
            var userId = int.Parse(id);
            try
            {
                OrderCard = await _orderService.GetRestaurantOrders(userId, status);
            }
             catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (OrderCard.Count > 0)
            {
                EmptyResult = false;
            }
            else
            {
                EmptyResult = true;
            }

        }

     
    }
}
