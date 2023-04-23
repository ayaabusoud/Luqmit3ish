using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    public class ResturantOrderDetailsViewModle : ViewModelBase
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

        public ResturantOrderDetailsViewModle(OrderCard order, INavigation navigation)
        {
            this._navigation = navigation;
            this._order = order;
            ProfileCommand = new Command<User>(async (User restaurant) => await OnProfileClicked(restaurant));
            _orderService = new OrderService();
            _foodService = new FoodServices();
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
