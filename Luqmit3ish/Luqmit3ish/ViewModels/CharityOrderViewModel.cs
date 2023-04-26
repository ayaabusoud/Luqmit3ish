using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class CharityOrderViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand EditCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }
        public ICommand OrderCommand { protected set; get; }
       
        private OrderService _orderService;

        private bool _emptyResult;

        public bool EmptyResult
        {
            get => _emptyResult;
            set => SetProperty(ref _emptyResult, value);
        }
 
        public CharityOrderViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            OrderCommand = new Command<OrderCard>(async (OrderCard order) => await OnFrameClicked(order));
            _orderService = new OrderService();
            OnInit();
        }

        private async Task OnFrameClicked(OrderCard order)
        {
            try
            {
                await _navigation.PushAsync(new  OrderDetailsPage(order));

            }catch(Exception e)
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
                        OnInit();
                        await PopupNavigation.Instance.PushAsync(new PopUp("The order have been deleted successfully."));
                        Thread.Sleep(3000);
                        await PopupNavigation.Instance.PopAsync();
                    
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new PopUp("The Order has not been deleted , please try again."));
                        Thread.Sleep(3000);
                        await PopupNavigation.Instance.PopAsync();
                    }
                }
                catch (Exception e)
                {

                    Debug.WriteLine(e.Message);
                    await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                }
            }

        }

        private ObservableCollection<OrderCard> _orderCards;

        public ObservableCollection<OrderCard> OrderCards
        {
            get => _orderCards;
            set => SetProperty(ref _orderCards, value);
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
                OrderCards = await _orderService.GetOrders(userId);
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
            if (OrderCards.Count > 0)
            {
              foreach(OrderCard order in OrderCards)
                {
                    if(order.Orders.Count == 1)
                    {
                        order.Items = "1 item";
                    }
                    else
                    {
                        order.Items = order.Orders.Count + " items";
                    }
                }
                EmptyResult = false;
            }
            else
            {
                EmptyResult = true;
            }
        }


       
    }
}
