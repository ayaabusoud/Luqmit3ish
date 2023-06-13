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

        private const string LowerKeepValidPrefKey = "LowerKeepValid";
        private const string UpperKeepValidPrefKey = "UpperKeepValid";
        private const string LowerQuantityPrefKey = "LowerQuantity";
        private const string UpperQuantityPrefKey = "UpperQuantity";
        private const string SelectedTypeValuesPrefKey = "SelectedTypeValues";
        private const string SelectedLocationValuesPrefKey = "SelectedLocationValues";
        private const string FilteredDishesPrefKey = "FilteredDishes";
        private const string NullReferenceExceptionMessage = "A null reference exception occurred. This can happen when a variable or object is null and you are trying to access or use it. Please check the input and try again.";
        private const string InvalidCastExceptionMessage = "An invalid cast exception occurred. This can happen when trying to convert a variable from one type to another that is not compatible. Please check the input and try again.";
        private const string InvalidOperationExceptionMessage = "An invalid operation exception occurred. This can happen when trying to perform an operation that is not valid or allowed in the current state of the program. Please check the input and try again.";
        private const string ExceptionsMessage = "An unexpected error occurred. Please try again or contact customer support for assistance.";

        public FilterFoodViewModel(INavigation navigation)
        {
            SelectedLocationValues = new ObservableCollection<object>();
            SelectedTypeValues = new ObservableCollection<object>();

            this._navigation = navigation;

            _typeValues = Constants.TypeValues;
            _locationValues = Constants.LocationValues;

            Apply = new Command(async () => await OnApplyAsync());
            ClearAll = new Command(async () => await OnClearAllAsync());
            TypeMultiSelectionCommand = new Command<IList<object>>(async (itemSelected) => await OnTypeMultiSelectionClicked(itemSelected));
            LocationMultiSelectionCommand = new Command<IList<object>>(async (itemSelected) => await OnLocationMultiSelectionClicked(itemSelected));

            _foodServices = new FoodServices();
            _userServices = new UserServices();
            InitializVariable();
        }

        private void InitializVariable()
        {
            _upperQuantity = Preferences.Get(UpperQuantityPrefKey, 100);
            _upperKeepValid = Preferences.Get(UpperKeepValidPrefKey, 10);
            _lowerQuantity = Preferences.Get(LowerQuantityPrefKey, 0);
            _lowerKeepValid = Preferences.Get(LowerKeepValidPrefKey, 0);

            var loadTypeSelectedValuesTask = LoadSelectedValues<TypeField>(SelectedTypeValuesPrefKey, SelectedTypeValues, NewSelectedTypeValues);
            var loadLocationSelectedValuesTask = LoadSelectedValues<LocationField>(SelectedLocationValuesPrefKey, SelectedLocationValues, NewSelectedLocationValues);

            Task.WaitAll(loadTypeSelectedValuesTask, loadLocationSelectedValuesTask);
        }

        private async Task LoadSelectedValues<T>(string preferencesKey, ObservableCollection<object> selectedValues, ObservableCollection<T> selectedItems) where T : class
        {
            try
            {
                string json = Preferences.Get(preferencesKey, string.Empty);

                if (!string.IsNullOrEmpty(json))
                {
                    selectedValues = JsonConvert.DeserializeObject<ObservableCollection<object>>(json);

                    if (selectedValues != null && selectedValues.Any())
                    {
                        var itemList = new ObservableCollection<T>(selectedValues.Select(o => JsonConvert.DeserializeObject<T>(o.ToString())));

                        foreach (var item in itemList)
                        {
                            selectedItems.Add(item);
                        }
                    }
                }
                else
                {
                    selectedValues = new ObservableCollection<object>();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task OnTypeMultiSelectionClicked(IList<object> itemSelected)
        {
            try
            {
                UpdateSelectedValues<TypeField>(itemSelected, TypeValues, _typeSelectedValues, x => x is TypeField);
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
                UpdateSelectedValues<LocationField>(itemSelected, LocationValues, _locationSelectedValues, x => x is LocationField);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                await PopNavigationAsync(NullReferenceExceptionMessage);
            }
            catch (InvalidCastException ex)
            {
                Debug.WriteLine(ex.Message);
                await PopNavigationAsync(InvalidCastExceptionMessage);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
                await PopNavigationAsync(InvalidOperationExceptionMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private void UpdateSelectedValues<T>(IList<object> itemSelected, ObservableCollection<T> valuesCollection, ObservableCollection<T> selectedValuesCollection, Func<object, bool> predicate) where T : ISelectable
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
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task OnClearAllAsync()
        {
            await ClearTypeValues();
            await ClearLocationValues();

            LowerKeepValid = LowerQuantity = 0;
            UpperKeepValid = 10;
            UpperQuantity = 100;
        }

        private async Task ClearValues<T>(Func<IEnumerable<T>> getValueItems, ObservableCollection<T> newSelectedValues, ObservableCollection<object> selectedValues) where T : ISelectable
        {
            await Task.Run(() =>
            {
                foreach (var item in getValueItems())
                {
                    item.IsSelected = false;
                }
                newSelectedValues.Clear();
                selectedValues.Clear();
            });
        }

        private async Task ClearTypeValues()
        {
            await ClearValues<TypeField>(() => TypeValues, NewSelectedTypeValues, SelectedTypeValues);
        }

        private async Task ClearLocationValues()
        {
            await ClearValues<LocationField>(() => LocationValues, NewSelectedLocationValues, SelectedLocationValues);
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

                ObservableCollection<DishCard> filteredDishes;

                if (_locationSelectedValues.Any())
                {
                    var restaurantIds = filterUsers
                        .Where(user => _locationSelectedValues.Any(loc => loc.Name == user.Location))
                        .Select(user => user.Id)
                        .ToList();

                    filteredDishes = new ObservableCollection<DishCard>(
                        allDishes.Where(dish =>
                            dish.Restaurant != null && restaurantIds.Contains(dish.Restaurant.Id)
                        )
                    );
                }
                else
                {
                    filteredDishes = allDishes;
                }

                filteredDishes = new ObservableCollection<DishCard>(
                    filteredDishes.Where(dish =>
                        dish.KeepValid >= filterInfo.LowerKeepValid && dish.KeepValid <= filterInfo.UpperKeepValid &&
                        dish.Quantity >= filterInfo.LowerQuantity && dish.Quantity <= filterInfo.UpperQuantity &&
                        (filterInfo.TypeValues == null || !filterInfo.TypeValues.Any() || filterInfo.TypeValues.Contains(dish.Type))
                    )
                );

                string dishes = JsonConvert.SerializeObject(filteredDishes);
                Preferences.Set("FilteedDishes", dishes);

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

        private async Task SavePreferences(ObservableCollection<DishCard> filteredDishes)
        {
            await Task.Run(() =>
            {
                Preferences.Set(LowerKeepValidPrefKey, LowerKeepValid);
                Preferences.Set(UpperKeepValidPrefKey, UpperKeepValid);
                Preferences.Set(LowerQuantityPrefKey, LowerQuantity);
                Preferences.Set(UpperQuantityPrefKey, UpperQuantity);

                SaveSelectedValues(SelectedTypeValuesPrefKey, NewSelectedTypeValues);
                SaveSelectedValues(SelectedLocationValuesPrefKey, NewSelectedLocationValues);

                if (filteredDishes?.Any() == true)
                {
                    string dishesJson = JsonConvert.SerializeObject(filteredDishes);
                    Preferences.Set(FilteredDishesPrefKey, dishesJson);
                }
                else
                {
                    Preferences.Remove(FilteredDishesPrefKey);
                }
            });
        }

        private void SaveSelectedValues<T>(string prefKey, ObservableCollection<T> selectedValues)
        {
            if (selectedValues?.Any() == true)
            {
                string json = JsonConvert.SerializeObject(selectedValues);
                Preferences.Set(prefKey, json);
            }
            else
            {
                Preferences.Remove(prefKey);
            }
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