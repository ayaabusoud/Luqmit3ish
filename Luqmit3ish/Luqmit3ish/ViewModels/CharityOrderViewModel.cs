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
        public ICommand DeleteCommand { protected set; get; }
        public Command<OrderCard> OrderCommand { protected set; get; }
       
        private OrderService _orderService;

        private bool _emptyResult;

        public bool EmptyResult
        {
            get => _emptyResult;
            set => SetProperty(ref _emptyResult, value);
        }

        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
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
                await _navigation.PushAsync(new OrderDetailsPage());

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
                    await App.Current.MainPage.DisplayAlert("Success", 
                        "The order have been deleted successfully", "ok");
                    OnInit();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Faild", 
                            "The Order has not been deleted , please try again", "ok");
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
