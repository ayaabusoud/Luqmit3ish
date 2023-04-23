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
            set => SetProperty(ref _order, value);
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
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command<int>(OnMinusClicked);
            ProfileCommand = new Command<User>(async (User restaurant) => await OnProfileClicked(restaurant));
            _orderService = new OrderService();
            _foodService = new FoodServices();
        }
  
    


     
        private  void OnMinusClicked(int orderId)
        {
            try
            {
                //    await _orderService.UpdateOrderDishCount(orderId, "Minus");
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

        private async void OnPlusClicked(int orderId)
        {
            try
            {
               
              //  await _orderService.UpdateOrderDishCount(orderId, "plus");
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
                await _navigation.PushAsync(new OtherProfilePage(restaurant));

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
