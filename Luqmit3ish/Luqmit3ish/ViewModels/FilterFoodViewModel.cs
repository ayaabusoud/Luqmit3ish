using System;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Utilities;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;
using Newtonsoft.Json;
using FFImageLoading;

namespace Luqmit3ish.ViewModels
{
    public class FilterFoodViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand Apply { get; set; }
        public ICommand ClearAll { get; set; }
        public ICommand TypeMultiSelectionCommand { get; set; }
        public ICommand LocationMultiSelectionCommand { get; set; }

        private IFoodServices _foodServices;
        private IUserServices _userServices;

        public FilterFoodViewModel(INavigation navigation)
        {
            SelectedLocationValues = new ObservableCollection<object>();
            this._navigation = navigation;

            _typeValues = Constants.TypeValues;
            _locationValues = Constants.LocationValues;

            Apply = new Command(async () => await OnApplyAsync());
            ClearAll = new Command(() => OnClearAllAsync());
            TypeMultiSelectionCommand = new Command<IList<object>>(async (itemSelected) => await OnTypeMultiSelectionClicked(itemSelected));
            LocationMultiSelectionCommand = new Command<IList<object>>(async (itemSelected) => await OnLocationMultiSelectionClicked(itemSelected));

            _foodServices = new FoodServices();
            _userServices = new UserServices();
            InitializVariable();
        }

        private void InitializVariable()
        {
            int upperQuantity = Preferences.Get("UpperQuantity", 0);
            _upperQuantity = upperQuantity == 0 ? 100 : upperQuantity;

            int upperKeepValid = Preferences.Get("UpperKeepValid", 0);
            _upperKeepValid = upperKeepValid == 0 ? 10 : upperKeepValid;

            int lowerQuantity = Preferences.Get("LowerQuantity", 0);
            _lowerQuantity = lowerQuantity == 0 ? 0 : lowerQuantity;

            int lowerKeepValid = Preferences.Get("LowerKeepValid", 0);
            _lowerKeepValid = lowerKeepValid == 0 ? 0 : lowerKeepValid;

            var LoadTypeSelectedValues = LoadSelectedValues<TypeField>("SelectedTypeValues", SelectedTypeValues, NewSelectedTypeValues);
            var LoadLocationSelectedValues = LoadSelectedValues<LocationField>("SelectedLocationValues", SelectedLocationValues, NewSelectedLocationValues);
            Task.WaitAll(LoadTypeSelectedValues, LoadLocationSelectedValues);
        }

        private async Task LoadSelectedValues<T>(string preferencesKey, ObservableCollection<object> selectedValues, ObservableCollection<T> selectedItems) where T : class
        {
            try
            {
                string json = Preferences.Get(preferencesKey, string.Empty);
                if (!string.IsNullOrEmpty(json))
                {
                    selectedValues = JsonConvert.DeserializeObject<ObservableCollection<object>>(json);
                    var itemList = new ObservableCollection<T>(
                        selectedValues.Select(o => JsonConvert.DeserializeObject<T>(o.ToString()))
                    );
                    foreach (var item in itemList)
                    {
                        selectedItems.Add(item);
                    }
                    ClearFilterUsers(itemList);
                }
                else
                {
                    selectedValues = new ObservableCollection<object>();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task OnTypeMultiSelectionClicked(IList<object> itemSelected)
        {
            try
            {
                await UpdateSelectedValues<TypeField>(itemSelected, TypeValues, _typeSelectedValues, x => x is TypeField);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task OnLocationMultiSelectionClicked(IList<object> itemSelected)
        {
            try
            {
                await UpdateSelectedValues<LocationField>(itemSelected, LocationValues, _locationSelectedValues, x => x is LocationField);
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(HttpRequestMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task UpdateSelectedValues<T>(IList<object> itemSelected, ObservableCollection<T> valuesCollection, ObservableCollection<T> selectedValuesCollection, Func<object, bool> predicate) where T : ISelectable
        {
            try
            {
                selectedValuesCollection.Clear();

                foreach (var item in itemSelected)
                {
                    if (predicate(item))
                    {
                        if (item is T selectedItems)
                        {
                            selectedItems.IsSelected = true;
                            selectedValuesCollection.Add(selectedItems);
                        }
                    }
                }

                foreach (var item in valuesCollection)
                {
                    if (item is T valueField && !selectedValuesCollection.Contains(valueField))
                    {
                        valueField.IsSelected = false;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private void OnClearAllAsync()
        {
            Parallel.Invoke(
                () => ClearTypeValuesAsync(),
                () => ClearLocationValues()
            );

            LowerKeepValid = LowerQuantity = 0;
            UpperKeepValid = 10;
            UpperQuantity = 100;
        }

        private void ClearTypeValuesAsync()
        {
            foreach (var item in TypeValues)
            {
                item.IsSelected = false;
            }
            NewSelectedTypeValues.Clear();
            SelectedTypeValues.Clear();
        }

        private void ClearLocationValues()
        {
            foreach (var item in LocationValues)
            {
                item.IsSelected = false;
            }
            NewSelectedLocationValues.Clear();
            SelectedLocationValues.Clear();
        }

        private async Task OnApplyAsync()
        {
            try
            {
                FilterInfo filterInfo = new FilterInfo();
                InitializFilterInfo(filterInfo);

                ObservableCollection<DishCard> allDishes = await _foodServices.GetDishCards();
                ObservableCollection<User> allUsers = await _userServices.GetUsers();

                ObservableCollection<User> filterUsers = new ObservableCollection<User>(
                    allUsers.Where(user => filterInfo.LocationValues.Any(location => user.Location.Contains(location)))
                );

                ObservableCollection<DishCard> filteredDishes = new ObservableCollection<DishCard>(
                    allDishes.Where(dish =>
                        dish.KeepValid >= filterInfo.LowerKeepValid && dish.KeepValid <= filterInfo.UpperKeepValid &&
                        dish.Quantity >= filterInfo.LowerQuantity && dish.Quantity <= filterInfo.UpperQuantity &&
                        (filterInfo.TypeValues == null || !filterInfo.TypeValues.Any() || filterInfo.TypeValues.Contains(dish.Type)) &&
                        (!filterUsers.Any() || filterUsers.Any(user => user.Id == dish.Restaurant.Id))
                    )
                );

                Parallel.Invoke(
                    () => ClearFilterUsers(filterUsers),
                    () => ClearFilterUsers(allDishes),
                    () => SetPreferences(filteredDishes)
                );
                
                await _navigation.PopAsync();
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(HttpRequestMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private void SetPreferences(ObservableCollection<DishCard> filteredDishes)
        {
            Preferences.Set("LowerKeepValid", LowerKeepValid);
            Preferences.Set("UpperKeepValid", UpperKeepValid);
            Preferences.Set("LowerQuantity", LowerQuantity);
            Preferences.Set("UpperQuantity", UpperQuantity);

            string typeJson = JsonConvert.SerializeObject(NewSelectedTypeValues);
            Preferences.Set("SelectedTypeValues", typeJson);

            string locationJson = JsonConvert.SerializeObject(NewSelectedLocationValues);
            Preferences.Set("SelectedLocationValues", locationJson);

            string dishes = JsonConvert.SerializeObject(filteredDishes);
            Preferences.Set("FilteedDishes", dishes);
        }

        private void InitializFilterInfo(FilterInfo filterInfo)
        {
            foreach (var item in NewSelectedTypeValues)
            {
                filterInfo.TypeValues.Add(item.Name);
            }
            foreach (var item in NewSelectedLocationValues)
            {
                filterInfo.LocationValues.Add(item.Name);
            }

            filterInfo.LowerKeepValid = _lowerKeepValid;
            filterInfo.LowerQuantity = _lowerQuantity;

            filterInfo.UpperKeepValid = _upperKeepValid;
            filterInfo.UpperQuantity = _upperQuantity;
        }

        private ObservableCollection<object> _selectedTypeValues;
        public ObservableCollection<object> SelectedTypeValues
        {
            get => _selectedTypeValues;
            set => SetProperty(ref _selectedTypeValues, value);
        }

        private ObservableCollection<object> _selectedLocationValues;
        public ObservableCollection<object> SelectedLocationValues
        {
            get => _selectedLocationValues;
            set => SetProperty(ref _selectedLocationValues, value);
        }

        private ObservableCollection<TypeField> _typeSelectedValues = new ObservableCollection<TypeField>();
        public ObservableCollection<TypeField> NewSelectedTypeValues
        {
            get => _typeSelectedValues;
            set => SetProperty(ref _typeSelectedValues, value);
        }

        private ObservableCollection<LocationField> _locationSelectedValues = new ObservableCollection<LocationField>();
        public ObservableCollection<LocationField> NewSelectedLocationValues
        {
            get => _locationSelectedValues;
            set => SetProperty(ref _locationSelectedValues, value);
        }

        private int _upperKeepValid;
        public int UpperKeepValid
        {
            get => _upperKeepValid;
            set => SetProperty(ref _upperKeepValid, value);
        }

        private int _lowerKeepValid;
        public int LowerKeepValid
        {
            get => _lowerKeepValid;
            set => SetProperty(ref _lowerKeepValid, value);
        }

        private int _lowerQuantity;
        public int LowerQuantity
        {
            get => _lowerQuantity;
            set => SetProperty(ref _lowerQuantity, value);
        }

        private int _upperQuantity;
        public int UpperQuantity
        {
            get => _upperQuantity;
            set => SetProperty(ref _upperQuantity, value);
        }

        private ObservableCollection<TypeField> _typeValues;
        public ObservableCollection<TypeField> TypeValues
        {
            get => _typeValues;
            set => SetProperty(ref _typeValues, value);
        }

        private ObservableCollection<LocationField> _locationValues;
        public ObservableCollection<LocationField> LocationValues
        {
            get => _locationValues;
            set => SetProperty(ref _locationValues, value);
        }
    }
}