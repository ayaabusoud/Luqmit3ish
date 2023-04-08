using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;
using Luqmit3ish.Services;
using System.Collections.ObjectModel;
using Luqmit3ish.Models;

namespace Luqmit3ish.ViewModels
{
    class CharityHomeViewModel : ViewModelBase
    {
        public INavigation Navigation { get; set; }
        public ICommand FilterCommand { protected set; get; }
        public ICommand SearchCommand { protected set; get; }
        public Command<int> PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public ICommand ReserveCommand { protected set; get; }
        public Command<int> ProfileCommand { protected set; get; }

        public FoodServices foodServices;
        public UserServices userServices; 

        private int _counter = 0;

        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
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
            this.Navigation = navigation;
            FilterCommand = new Command(async () => await OnFilterClicked());
            SearchCommand = new Command(async () => await OnSearchClicked());
            ProfileCommand = new Command<int>(async (int restaurantId) => await OnProfileClicked(restaurantId));
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            ReserveCommand = new Command(OnReserveClicked);
            foodServices = new FoodServices();
            userServices = new UserServices(); 
            OnInit();
        }
        private void OnReserveClicked()
        {
           //imp
        }

        private void OnMinusClicked()
        {
            if (Counter == 0)
            {
                return;
            }
            else
            {
              Counter--;
            }
           
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
               DishCard = await foodServices.GetDishCards();
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async Task OnFilterClicked()
        {
            try
            {
                await Navigation.PushAsync(new FilterFoodPage());
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
                await Navigation.PushAsync(new SearchPage());

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
                await Navigation.PushAsync(new OtherProfilePage(restaurantId));

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