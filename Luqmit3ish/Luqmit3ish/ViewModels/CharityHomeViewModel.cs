using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;



namespace Luqmit3ish.ViewModels
{
    class CharityHomeViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand FilterCommand { protected set; get; }
        public ICommand SearchCommand { protected set; get; }
        public ICommand FoodDetailCommand { protected set; get; }
        private readonly string _noFilterTitle = "No Filter Match Found";
        private readonly string _noFilterDescription = "There is no food matches the filters you're looking for!";
        private readonly string _noFoodTitle = "No Food Available";
        private readonly string _noFoodDescription = "Come back later to explore new food!";

        private readonly IFoodServices _foodServices;


        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private ObservableCollection<DishCard> _dishCards;

        public ObservableCollection<DishCard> DishCards
        {
            get => _dishCards;
            set => SetProperty(ref _dishCards, value);
        }
        private bool _emptyResult;

        public bool EmptyResult
        {
            get => _emptyResult;
            set => SetProperty(ref _emptyResult, value);
        }


        public CharityHomeViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            FilterCommand = new Command(async () => await OnFilterClicked());
            SearchCommand = new Command(async () => await OnSearchClicked());
            FoodDetailCommand = new Command<DishCard>(async (DishCard dish) => await OnFrameClicked(dish));
            _foodServices = new FoodServices();
            OnInit();
        }

        private async Task OnFrameClicked(DishCard dish)
        {
            try
            {
                await _navigation.PushAsync(new FoodDetailPage(dish));
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


        private void OnInit()
        {
            try
            {


                Task.Run(async () => {
                    try
                    {
                        string dishesJson = Preferences.Get("FilteedDishes", string.Empty);

                        if (!string.IsNullOrEmpty(dishesJson))
                        {
                            ObservableCollection<DishCard> allDishesUpdated = await _foodServices.GetDishCards();
                            ObservableCollection<DishCard> filterDishes = JsonConvert.DeserializeObject<ObservableCollection<DishCard>>(dishesJson);

                            var dishCardIds = filterDishes.Select(dc => dc.Id);

                            var filteredDishes = allDishesUpdated.Where(d => dishCardIds.Contains(d.Id));

                            DishCards = new ObservableCollection<DishCard>(filteredDishes);


                            if (DishCards.Count > 0)
                            {
                                EmptyResult = false;
                                RemoveEmptyDish();
                            }
                            else
                            {
                                EmptyResult = true;
                                Title = _noFilterTitle;
                                Description = _noFilterDescription ;
                            }
                        }
                        else
                        {
                            DishCards = await _foodServices.GetDishCards();

                            if (DishCards.Count > 0)
                            {
                                EmptyResult = false;
                                RemoveEmptyDish();
                            }
                            else
                            {
                                EmptyResult = true;
                                Title = _noFoodTitle;
                                Description = _noFoodDescription;
                            }
                        }
                    }
                    catch (ConnectionException e)
                    {
                        Debug.WriteLine(e.Message);
                        await PopNavigationAsync(InternetMessage);
                    }
                    catch (HttpRequestException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }

                }).Wait();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        private void RemoveEmptyDish()
        {
            foreach (DishCard dish in DishCards)
            {
                if (dish.Quantity == 0)
                {
                    DishCards.Remove(dish);
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
    }
}