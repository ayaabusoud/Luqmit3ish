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
        private OrderService _orderService;


        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        public RestaurantOrderViewModel(INavigation navigation)
        {
           _navigation = navigation;
            ExpanderCommand = new Command<int>(OnExpanderClicked);
            DoneCommand = new Command<int>(async (int OrderId) => await OnDoneClick(OrderId));
            _orderService = new OrderService();
            OnInit();
        }

        private async Task OnDoneClick(int id)
        {

            var charity = OrderCard.FirstOrDefault(i => i.id == id);
            var numberOfCharityOrders = charity.data.Count;
            for (int i = numberOfCharityOrders - 1; i >= 0; i--)
            {
                await _orderService.UpdateOrderReceiveStatus(charity.data[i].id);
                charity.data.RemoveAt(i);
            }
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
   

        private ObservableCollection<OrderCard> _orderCard;

        public ObservableCollection<OrderCard> OrderCard
        {
            get => _orderCard;
            set => SetProperty(ref _orderCard, value);
        }

        private async void OnInit()
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
                OrderCard = await _orderService.GetRestaurantOrders(userId, false);
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
