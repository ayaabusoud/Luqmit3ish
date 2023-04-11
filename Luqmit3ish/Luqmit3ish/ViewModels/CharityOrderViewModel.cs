using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


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
        public ICommand DeleteCommand { protected set; get; }
        public Command<int> PlusCommand { protected set; get; }
        public Command<int> MinusCommand { protected set; get; }

        public CharityOrderViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            EditCommand = new Command<int>(async (int id) => await OnEditClicked(id));
            DeleteCommand = new Command<int>(async (int id) => await OnDeleteClicked(id));
            Search = new Command(async () => await OnSearchClicked());
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command<int>(OnMinusClicked);
            orderService = new OrderService();
            OnInit();
        }
        private void OnMinusClicked(int orderId)
        {
           
        }

        private void OnPlusClicked(int orderId)
        {
            
        }

        private async Task OnSearchClicked()
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async Task OnDeleteClicked(int Restaurantid)
        {
            //imp
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
