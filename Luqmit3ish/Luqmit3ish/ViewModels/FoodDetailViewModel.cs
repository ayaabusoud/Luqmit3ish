using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class FoodDetailViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public ICommand ReserveCommand { protected set; get; }
        public ICommand ProfileCommand { protected set; get; }
        private OrderService _orderService;


        public FoodDetailViewModel(DishCard dish, INavigation navigation)
        {
            this._navigation = navigation;
            ProfileCommand = new Command<User>(async (User restaurant) => await OnProfileClicked(restaurant));
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            ReserveCommand = new Command(async () => await OnReserveClicked());
            _orderService = new OrderService();
            this._dishInfo = dish;
        }

        private int _counter = 1;

        public int Counter
        {
            get => _counter;
            set
            {
                SetProperty(ref _counter, value);
            }
        }

        private DishCard _dishInfo;

        public DishCard DishInfo
        {
            get => _dishInfo;
            set => SetProperty(ref _dishInfo, value);
        }

        private string _plusColor = "Orange";
        public string PlusColor
        {
            get => _plusColor;
            set => SetProperty(ref _plusColor, value);
        }
        private string _minusColor;
        public string MinusColor
        {
            get => _minusColor;
            set => SetProperty(ref _minusColor, value);
        }

        private void OnMinusClicked()
        {
            if (Counter == 1)
            {
                MinusColor = "Gray";
                PlusColor = "Orange";
                return;
            }
            if (Counter < 1)
            {
                Counter = 1;
                MinusColor = "Gray";
                PlusColor = "Orange";
                return;
            }

            Counter--;
            if (Counter == 1)
            {
                MinusColor = "Gray";
                PlusColor = "Orange";
                return;
            }
            if (Counter > 1)
            {
                PlusColor = "Orange";
                MinusColor = "Orange";
            }
        }

       
        private void OnPlusClicked(int quantity)
        {
            if (Counter == quantity)
            {
                return;
            }

            Counter++;

            if (Counter == quantity)
            {
                PlusColor = "Gray";
                MinusColor = "Orange";
                return;
            }
            if(Counter > 1)
            {
                PlusColor = "Orange";
                MinusColor = "Orange";
                return;
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

    
        private async Task OnReserveClicked()
        {
            try
            {
                int UserId = GetUserId();

                Order newOrder = CreateNewOrder(UserId);

                await _orderService.ReserveOrder(newOrder);

                await _navigation.PopAsync();
                await PopNavigationAsync("Your order has been successfully reserved");

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
                await PopNavigationAsync(NotAuthorizedMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }
        Order CreateNewOrder(int UserId)
        {
            Order newOrder = new Order();
            newOrder.CharId = UserId;
            newOrder.ResId = _dishInfo.Restaurant.Id;
            newOrder.DishId = _dishInfo.Id;
            newOrder.Date = DateTime.Now;
            newOrder.Quantity = Counter;
            newOrder.Receive = false;
            return newOrder;
        }
    }
}
