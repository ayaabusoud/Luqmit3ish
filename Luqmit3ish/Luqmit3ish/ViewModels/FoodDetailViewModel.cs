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
        private ObservableCollection<DishCard> _dishCard;

        public ObservableCollection<DishCard> DishCard
        {
            get => _dishCard;
            set => SetProperty(ref _dishCard, value);
        }
        private async Task OnReserveClicked(int FoodId)
        {

        }
        public FoodDetailViewModel(int id, INavigation navigation)
        {
            this._navigation = navigation;
            ProfileCommand = new Command<int>(async (int restaurantId) => await OnProfileClicked(restaurantId));
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            ReserveCommand = new Command<int>(async (int FoodId) => await OnReserveClicked(FoodId));
            _foodServices = new FoodServices();
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