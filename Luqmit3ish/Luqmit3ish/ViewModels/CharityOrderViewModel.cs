using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand EditCommand { protected set; get; }
        public ICommand Search { protected set; get; }

        public ICommand ProfileCommand { protected set; get; }
        public OrderService orderService;
        public FoodServices foodService;

        public ICommand DeleteCommand { protected set; get; }
        public Command<int> PlusCommand { protected set; get; }
        public Command<int> MinusCommand { protected set; get; }


        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        public CharityOrderViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            EditCommand = new Command<int>(async (int id) => await OnEditClicked(id));
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            Search = new Command(async () => await OnSearchClicked());
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command<int>(OnMinusClicked);
            ExpanderCommand = new Command<int>(OnExpanderClicked);
            orderService = new OrderService();
            foodService = new FoodServices();
            OnInit();
        }


        private async Task OnSearchClicked()
        {
            await Navigation.PushAsync(new SearchPage());
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
        private async void OnMinusClicked(int orderId)
        {
            await orderService.UpdateOrderDishCount(orderId, "Minus");
            var order = await orderService.GetOrderById(orderId);
            Dish dish = await foodService.GetFoodById(order.dish_id);
            var id = Preferences.Get("userId", null);
            var userId = int.Parse(id);
            OrderCard = await orderService.GetOrders(userId);
            foreach (OrderCard item in OrderCard)
            {
                if (item.id == dish.user_id)
                {
                    item.IsExpanded = true;
                }
            }


        }

        private async void OnPlusClicked(int orderId)
        {
            var order = await orderService.GetOrderById(orderId);
            Dish dish = await foodService.GetFoodById(order.dish_id);
            if (dish.number == 0)
            {
                return;
            }
            await orderService.UpdateOrderDishCount(orderId, "plus");
            var id = Preferences.Get("userId", null);
            var userId = int.Parse(id);
            OrderCard = await orderService.GetOrders(userId);
            foreach (OrderCard item in OrderCard)
            {
                if (item.id == dish.user_id)
                {
                    item.IsExpanded = true;
                }
            }

        }

private async Task OnDeleteClicked(int restaurantId)
        {
            var deleteConfirm = await Application.Current.MainPage.DisplayAlert("Delete Order", "Are you sure that you want to delete this Order?", "Yes", "No");
            if (deleteConfirm)
            {
                var id = Preferences.Get("userId", null);
                var userId = int.Parse(id);
                try
                {
                    await orderService.DeleteOrder(userId, restaurantId);
                    await App.Current.MainPage.DisplayAlert("Success", "The order have been deleted successfully", "ok");
                    OnInit();
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
            var userId = int.Parse(id);
            OrderCard = await orderService.GetOrders(userId);
            if(OrderCard is null)
            {
                //no orders yet
            }

            
        }

        private async Task OnEditClicked(int Restaurantid)
        {
            try
            {
            await Navigation.PushAsync(new EditOrderPage());

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
        private async Task OnProfileClicked()
        {

            try
            {
                await Navigation.PushAsync(new OtherProfilePage(1));

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
