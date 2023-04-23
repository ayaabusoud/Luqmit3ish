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
using System.Net.Http;
using Luqmit3ish.Exceptions;

namespace Luqmit3ish.ViewModels
{
    class SearchViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public FoodServices foodServices;
        public UserServices userServices;

        public Command<DishCard> FoodDetailCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

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
            foodServices = new FoodServices();
            FoodDetailCommand = new Command<DishCard>(async (DishCard dish) => await OnFrameClicked(dish));
            userServices = new UserServices();
            OnInit();
        }

        private void OnBackClicked()
        {
            _navigation.PopAsync();
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


        private async Task OnInit()
        {
            try
            {
                DishCard = await foodServices.GetSearchCards(_searchText, "All");
                Debug.WriteLine(DishCard.Count);          
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
            if (DishCard.Count > 0)
            {
                EmptyResult = false;
                foreach (DishCard dish in DishCard)
                {
                    if (dish.Quantity == 0)
                    {
                        DishCard.Remove(dish);
                    }
                }
            }
            else
            {
                EmptyResult = true;
          
            }

        }
        private async Task OnFrameClicked(DishCard dish)
        {
            try
            {
                await _navigation.PushAsync(new FoodDetailPage(dish.Id));
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