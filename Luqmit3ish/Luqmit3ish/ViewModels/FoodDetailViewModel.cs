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
    class FoodDetailViewModel: ViewModelBase
    {
    private INavigation _navigation { get; set; }
        public Command<int> PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public Command<int> ReserveCommand { protected set; get; }
        public Command<int> ProfileCommand { protected set; get; }
        private FoodServices _foodServices;
        private UserServices _userServices;
        private OrderService _orderService;

        private int _counter = 0;
        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        private String _restaurantImg;
        public String RestaurantImg
        {
            get => _restaurantImg;
            set => SetProperty(ref _restaurantImg, value);
        }
        private String _restaurantName;
        public String RestaurantName
        {
            get => _restaurantName;
            set => SetProperty(ref _restaurantName, value);
        }
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
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }
        private String _image;
        public String Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
        private int _keepValid;
        public int KeepValid
        {
            get => _keepValid;
            set => SetProperty(ref _keepValid, value);
        }
        private String _description;
        public String Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        private String _pickUp;
        public String PickUp
        {
            get => _pickUp;
            set => SetProperty(ref _pickUp, value);
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
        private ObservableCollection<DishCard> _dishCard;

        public ObservableCollection<DishCard> DishCard
        {
            get => _dishCard;
            set => SetProperty(ref _dishCard, value);
        }
        private async Task OnReserveClicked(int FoodId)
        {
            
        }
        public FoodDetailViewModel (int id , INavigation navigation)
        {
            this._navigation = navigation;
            ProfileCommand = new Command<int>(async (int restaurantId) => await OnProfileClicked(restaurantId));
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            ReserveCommand = new Command<int>(async (int FoodId) => await OnReserveClicked(FoodId));
            _foodServices = new FoodServices();
            _orderService = new OrderService();
            _userServices = new UserServices();
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
            if(Dish != null)
               
            {
                
                PickUp = Dish[0].pickUpTime;
                Image = Dish[0].photo;
                Description = Dish[0].description;
                KeepValid = Dish[0].keepValid;
                Quantity = Dish[0].quantity;
                RestaurantImg = Dish[0].RestaurantImage;
                RestaurantName = Dish[0].restaurantName; 
            }
            if(RestaurantImg == null || RestaurantImg == "")
            {
                RestaurantImg = "Luqma.jpg"; 
            }

        }
    }
}
