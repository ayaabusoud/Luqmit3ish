using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
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

        private IFoodServices _foodServices;


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
                        DishCards = JsonConvert.DeserializeObject<ObservableCollection<DishCard>>(dishesJson);
                        if (DishCards.Count > 0)
                        {
                            EmptyResult = false;
                            RemoveEmptyDish();
                        }
                        else
                        {
                            EmptyResult = true;
                            Title = "No Filter Match Found";
                            Description = "There is no food matches the filters you're looking for!";
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
                            Title = "No Food Available";
                            Description = "Come back later to explore new food!";
                        }
                    }
                }
                catch (ConnectionException e)
                {
                    Debug.WriteLine(e.Message);
                    await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
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

            }catch(Exception e)
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
                else if (dish.Quantity == 1)
                {
                    dish.Items = "1 Dish";
                }
                else
                {
                    dish.Items = dish.Quantity + " Dishes";
                }
            }
        }

        private async Task OnFilterClicked()
        {
            try
            {
                await _navigation.PushAsync(new FilterFoodPage());
                //await PopupNavigation.Instance.PushAsync(new FilterPopUp());
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