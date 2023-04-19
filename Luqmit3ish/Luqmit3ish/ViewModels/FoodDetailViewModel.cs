using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class FoodDetailViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public Command<int> PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public Command<int> ReserveCommand { protected set; get; }
        public Command<int> ProfileCommand { protected set; get; }
        private FoodServices _foodServices;
        private OrderService _orderService;

        private bool _isEnabled = false;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private int _counter = 0;

        public int Counter
        {
            get => _counter;
            set
            {
                SetProperty(ref _counter, value);
                if (_counter > 0)
                {
                    IsEnabled = true;
                }
                if (_counter == 0)
                {
                    IsEnabled = false;
                }
            }
        }
        private ObservableCollection<DishCard> _dish;
        public ObservableCollection<DishCard> Dish
        {
            get => _dish;
            set => SetProperty(ref _dish, value);
        }

        private void OnMinusClicked()
        {
            if (Counter == 0)
            {
                return;
            }
            if (Counter < 0)
            {
                Counter = 0;
                return;
            }

            Counter--;
        }

        private void OnPlusClicked(int quantity)
        {
            if (Counter == quantity)
            {
                return;
            }
            else
            {
                Counter++;
            }
        }
        private async Task OnProfileClicked(int restaurantId)
        {
            try
            {
                await _navigation.PushAsync(new OtherProfilePage(restaurantId));

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
        private ObservableCollection<DishCard> _dishCard ;

        public ObservableCollection<DishCard> DishCard
        {
            get => _dishCard;
            set => SetProperty(ref _dishCard, value);
        }
      

        private async Task OnReserveClicked(int FoodId)
        {
            try
            {
                var id = Preferences.Get("userId", "null");
                int UserId = int.Parse(id);
                Dish dish = await _foodServices.GetFoodById(FoodId);

                Order newOrder = new Order();
                newOrder.char_id = UserId;
                newOrder.res_id = dish.user_id;
                newOrder.dish_id = dish.id;
                newOrder.date = DateTime.Now;
                newOrder.number_of_dish = Counter;
                newOrder.receive = false;

                await _orderService.ReserveOrder(newOrder);

                //DishCard quantityDish = _dishCard.FirstOrDefault(d => d.id == dish.id);
                //if (quantityDish != null)
                //{
                //    quantityDish.quantity -= Counter;
                //}

                //if (Counter > 0) Counter = 0;

                //DishCard = await _foodServices.GetDishCards();

                //foreach (DishCard item in DishCard)
                //{
                //    if (item.quantity == 0)
                //    {
                //        DishCard.Remove(item);
                //    }
                //}
                await OnInit(FoodId);
                await App.Current.MainPage.DisplayAlert("successfuly", "Your order has been successfully booked", "ok");

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "ok");
            }

        }

        public FoodDetailViewModel(int id, INavigation navigation)
        {
            this._navigation = navigation;
            ProfileCommand = new Command<int>(async (int restaurantId) => await OnProfileClicked(restaurantId));
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            ReserveCommand = new Command<int>(async (int FoodId) => await OnReserveClicked(FoodId));
            _foodServices = new FoodServices();
            _orderService = new OrderService();
            OnInit(id);
        }


        private async Task OnInit(int id)
        {
            try
            {
                Dish = await _foodServices.GetDishCardById(id);
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
    }
}
