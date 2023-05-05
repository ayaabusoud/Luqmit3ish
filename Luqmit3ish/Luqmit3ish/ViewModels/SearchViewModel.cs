using Luqmit3ish.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using Luqmit3ish.Services;
using System.Collections.ObjectModel;
using Luqmit3ish.Models;
using System.Net.Http;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;

namespace Luqmit3ish.ViewModels
{
    class SearchViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public IFoodServices _foodServices;
        public ICommand FoodDetailCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
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

        public SearchViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            BackCommand = new Command(OnBackClicked);
            _foodServices = new FoodServices();
            FoodDetailCommand = new Command<DishCard>(async (DishCard dish) => await OnFrameClicked(dish));
           
            OnInit();
        }

        private void OnBackClicked()
        {
            try
            {
                _navigation.PopAsync();
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


        private void OnInit()
        {
            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        DishCards = await _foodServices.GetSearchCards(_searchText, "Dishes");
                        Debug.WriteLine(DishCards.Count);
                    }
                    catch (ConnectionException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    catch (HttpRequestException e)
                    {
                        throw new HttpRequestException(e.Message);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    if (DishCards.Count > 0)
                    {
                        EmptyResult = false;
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
                    else
                    {
                        EmptyResult = true;

                    }

                }).Wait();
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

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



    }
}