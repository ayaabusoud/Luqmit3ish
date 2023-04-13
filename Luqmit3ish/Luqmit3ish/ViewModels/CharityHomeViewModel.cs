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
    class CharityHomeViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand FilterCommand { protected set; get; }
        public ICommand SearchCommand { protected set; get; }
        public Command<int> PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public Command<int> ReserveCommand { protected set; get; }
        public Command<int> ProfileCommand { protected set; get; }

        private FoodServices _foodServices;
        private UserServices _userServices; 
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
                if(_counter > 0)
                {
                    IsEnabled = true;
                }
                if(_counter == 0)
                {
                    IsEnabled = false;
                }
            }
        }

        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        private ObservableCollection<DishCard> _dishCard;

        public ObservableCollection<DishCard> DishCard
        {
            get => _dishCard;
            set => SetProperty(ref _dishCard, value);
        }

        public  CharityHomeViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            FilterCommand = new Command(async () => await OnFilterClicked());
            SearchCommand = new Command(async () => await OnSearchClicked());
            ProfileCommand = new Command<int>(async (int restaurantId) => await OnProfileClicked(restaurantId));
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            ReserveCommand = new Command<int>(async (int FoodId) => await OnReserveClicked(FoodId));
            _foodServices = new FoodServices();
            ExpanderCommand = new Command<int>(OnExpanderClicked);
            _orderService = new OrderService();
            _userServices = new UserServices(); 
            OnInit();
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
            var item = DishCard.FirstOrDefault(i => i.id == id);
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

                DishCard quantityDish = _dishCard.FirstOrDefault(d => d.id == dish.id);
                if (quantityDish != null)
                {
                    quantityDish.quantity -=Counter;
                }

                if (Counter > 0) Counter=0;

                DishCard = await _foodServices.GetDishCards();
                
                foreach(DishCard item in DishCard)
                {
                    if(item.quantity == 0)
                    {
                        DishCard.Remove(item);
                    }
                    if (item.id == FoodId)
                    {
                        item.IsExpanded = true;
                    }
                }
                await App.Current.MainPage.DisplayAlert("successfuly", "Your order has been successfully booked", "ok");

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "ok");
            }
           
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
            if(Counter == quantity)
            {
                return;
            }
            else
            {
               Counter++;
            }
            
        }

        private async Task OnInit()
        {
            try
            {
                DishCard = await _foodServices.GetDishCards();
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
            if (DishCard != null)
            {
                foreach (DishCard dish in DishCard)
                {
                    if (dish.quantity == 0)
                    {
                        DishCard.Remove(dish);
                    }
                }
            }

        }

        private async Task OnFilterClicked()
        {
            try
            {
                await _navigation.PushAsync(new FilterFoodPage());
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
        private async Task OnSearchClicked()
        {
            try
            {
                await _navigation.PushAsync(new SearchPage());

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
     
    }
}

