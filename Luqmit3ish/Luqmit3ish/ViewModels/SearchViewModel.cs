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
    class SearchViewModel : ViewModelBase
    {
        public INavigation Navigation { get; set; }
        public FoodServices foodServices;
        public UserServices userServices;
        public Command<int> PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public ICommand AllFilter { protected set; get; }
        public ICommand RestaurantFilter { protected set; get; }
        public ICommand BackCommand { protected set; get; }


        public ICommand CharitiesFilter { protected set; get; }
        public Command<int> ProfileCommand { protected set; get; }
        private string _allColor = "#F98836";
        public string AllColor
        {
            get => _allColor;
            set => SetProperty(ref _allColor, value);
        }
        private string _restaurantColor = "Black";
        public string RestaurantColor
        {
            get => _restaurantColor;
            set => SetProperty(ref _restaurantColor, value);
        }
        private string _charityColor = "Black";
        public string CharityColor
        {
            get => _charityColor;
            set => SetProperty(ref _charityColor, value);
        }

        private int _counter = 0;
        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
        }
        private string _filter = "All";
        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
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

        public Command<int> ReserveCommand { protected set; get; }

        public SearchViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            ProfileCommand = new Command<int>(async (int restaurantId) => await OnProfileClicked(restaurantId));
            PlusCommand = new Command<int>(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            ReserveCommand = new Command<int>(async (int FoodId) => await OnReserveClicked(FoodId));
            AllFilter = new Command(OnAllClicked);
            RestaurantFilter = new Command(OnRestaurantClicked);
            BackCommand = new Command(OnBackClicked);
            CharitiesFilter = new Command(OnCharitiesClicked);
            foodServices = new FoodServices();
            userServices = new UserServices();
            OnInit();
        }

        private void OnBackClicked()
        {
            Navigation.PopAsync();
        }

        private string _searchText = string.Empty;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    SetProperty(ref _searchText, value);

                    // Perform the search
                    if (SearchCommand.CanExecute(null))
                    {
                        SearchCommand.Execute(null);
                    }
                }
            }
        }
        #region Command and associated methods for SearchCommand
        private Xamarin.Forms.Command _searchCommand;
        public System.Windows.Input.ICommand SearchCommand
        {
            get
            {
                _searchCommand = _searchCommand ?? new Xamarin.Forms.Command(DoSearchCommand, CanExecuteSearchCommand);
                return _searchCommand;
            }
        }
        private void DoSearchCommand()
        {
            // Refresh the list, which will automatically apply the search text

            OnInit();
        }
        private bool CanExecuteSearchCommand()
        {
            return true;
        }
        #endregion

        private async Task OnReserveClicked(int FoodId)
        {
            //imp
        }
        private void OnAllClicked()
        {
            Filter = "All";
            AllColor = "#F98836";
            RestaurantColor = "Black";
            CharityColor = "Black";
            OnInit();
        }
        private void OnRestaurantClicked()
        {
            Filter = "Restaurants";
            AllColor = "Black";
            RestaurantColor = "#F98836";
            CharityColor = "Black";
            OnInit();
        }
        private void OnCharitiesClicked()
        {
            Filter = "Dishes";
            AllColor = "Black";
            RestaurantColor = "Black";
            CharityColor = "#F98836";
            OnInit();
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
            if (Counter == quantity)
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
                DishCard = await foodServices.GetSearchCards(_searchText, Filter);
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