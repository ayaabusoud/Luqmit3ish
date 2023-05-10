using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Luqmit3ish.Interfaces;

namespace Luqmit3ish.ViewModels
{
    class RestaurantOrderViewModel : ViewModelBase
    {
          private INavigation _navigation { get; set; }

        public ICommand ProfileCommand { protected set; get; }
        public ICommand DoneCommand { protected set; get; }
        public ICommand NotRecievedCommand { protected set; get; }
        public ICommand RecievedCommand { protected set; get; }
        private IOrderService _orderService;
        public ICommand OrderCommand { protected set; get; }


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
            NotRecievedTextColor = Color.LightGray;
            RecievedTextColor = Color.Black;
            Selected(true);
            ReceievedCheck = false;
        }

        private void OnNotRecievedClicked()
        {
            IsVisible = true;
            NotRecievedColor = "DarkOrange";
            RecievedColor = "#D9D9D9";
            RecievedTextColor = Color.LightGray;
            NotRecievedTextColor = Color.Black;
            Selected(false);
            ReceievedCheck = true;
        }

        private async Task OnFrameClicked(OrderCard order)
        {
            try
            {
                await _navigation.PushAsync(new ResturantOrderDetailsPage(order));

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

        private async Task OnDoneClick(OrderCard orders)
        {
            try
            {
                foreach (OrderDish order in orders.Orders)
                {
                    await _orderService.UpdateOrderReceiveStatus(order.Id);
                }
                Selected(false);
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
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
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(NotAuthorizedMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }


   

        private ObservableCollection<OrderCard> _orderCards;

        public ObservableCollection<OrderCard> OrderCards
        {
            get => _orderCards;
            set => SetProperty(ref _orderCards, value);
        }

        private Color  _notRecievedTextColor = Color.Black;
        public Color NotRecievedTextColor
        {
            get => _notRecievedTextColor;
            set => SetProperty(ref _notRecievedTextColor, value);
        }

        private Color _recievedTextColor;
        public Color RecievedTextColor
        {
            get => _recievedTextColor;
            set => SetProperty(ref _recievedTextColor, value);
        }

        private void Selected(bool status)
        {
            try
            {


                Task.Run(async () => {
                var id = Preferences.Get("userId", null);
            if (id == null)
                {
                await PopupNavigation.Instance.PushAsync(new PopUp("Your login session has been expired."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
                   await _navigation.PushAsync(new LoginPage());
                    return;
                }
            var userId = int.Parse(id);
            try
            {
                OrderCards = await _orderService.GetRestaurantOrders(userId, status);
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
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(NotAuthorizedMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            if (OrderCards.Count > 0)
            {
                EmptyResult = false;
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

     
    }
}

