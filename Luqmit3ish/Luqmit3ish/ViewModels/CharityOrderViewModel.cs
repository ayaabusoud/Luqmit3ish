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
        public ICommand ProfileCommand { protected set; get; }
        public OrderService orderService;
        public ICommand DeleteCommand { protected set; get; }

        public CharityOrderViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            EditCommand = new Command(async () => await OnEditClicked());
            ProfileCommand = new Command(async () => await OnProfileClicked());
            DeleteCommand = new Command(OnDeleteClicked);
            orderService = new OrderService();
            OnInit();
        }

        private void OnDeleteClicked()
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

            
        }

        private async Task OnEditClicked()
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
