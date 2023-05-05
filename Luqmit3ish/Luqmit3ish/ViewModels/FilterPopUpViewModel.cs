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

namespace Luqmit3ish.ViewModels
{
	public class FilterPopUpViewModel: ViewModelBase
	{
        private INavigation _navigation { get; set; }
        public ICommand Apply { get; set; }
        public ICommand ClearAll { get; set; }
        public ICommand CancelCommand { get; set; }
        private IFoodServices _foodServices;
        private IUserServices _userServices;

        public FilterPopUpViewModel(INavigation navigation)
        {
            //SelectedTypeValues = new ObservableCollection<object>();
            SelectedLocationValues = new ObservableCollection<object>();
            this._navigation = navigation;

            _typeValues = Constants.TypeValues;
            _locationValues = Constants.LocationValues;

            Apply = new Command(async () => await OnApplyAsync());
            ClearAll = new Command(async () => await OnClearAllAsync());
            CancelCommand = new Command(async () => await OnCancelAsync());
            InitializVariable();
        }

        private void InitializVariable()
        {
            int upperQuantity = Preferences.Get("UpperQuantity", 0);
            if (upperQuantity == 0)
            {
                _upperQuantity = 100;
            }
            else
            {
                _upperQuantity = upperQuantity;
            }
            int upperKeepValid = Preferences.Get("UpperKeepValid", 0);
            if (upperKeepValid == 0)
            {
                _upperKeepValid = 10;
            }
            else
            {
                _upperKeepValid = upperKeepValid;
            }

            int lowerQuantity = Preferences.Get("LowerQuantity", 0);
            if (lowerQuantity == 0)
            {
                _lowerQuantity = 0;
            }
            else
            {
                _lowerQuantity = lowerQuantity;
            }

            int lowerKeepValid = Preferences.Get("LowerKeepValid", 0);
            if (lowerKeepValid == 0)
            {
                _lowerKeepValid = 0;
            }
            else
            {
                _lowerKeepValid = lowerKeepValid;
            }

            string typeJson = Preferences.Get("SelectedTypeValues", string.Empty);

            if (!string.IsNullOrEmpty(typeJson))
            {
                SelectedTypeValues = JsonConvert.DeserializeObject<ObservableCollection<object>>(typeJson);
            }
            else
            {
                SelectedTypeValues = new ObservableCollection<object>();
            }

            string locationJson = Preferences.Get("SelectedLocationValues", string.Empty);

            if (!string.IsNullOrEmpty(locationJson))
            {
                SelectedLocationValues = JsonConvert.DeserializeObject<ObservableCollection<object>>(locationJson);
            }
            else
            {
                SelectedLocationValues = new ObservableCollection<object>();
            }

            _foodServices = new FoodServices();
            _userServices = new UserServices();
           
        }

        public ICommand TypeMultiSelectionCommand => new Command<IList<object>>(async (itemSelected) =>
        {
            try
            {
                _typeSelectedValues.Clear();

                foreach (var item in itemSelected)
                {
                    if (item is TypeField selectedItems)
                    {
                        selectedItems.IsSelected = true;
                        _typeSelectedValues.Add(selectedItems);
                    }
                }

                foreach (var item in TypeValues)
                {
                    if (item is TypeField typeField && !_typeSelectedValues.Contains(typeField))
                    {
                        typeField.IsSelected = false;
                    }
                }
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        });

        public ICommand LocationMultiSelectionCommand => new Command<IList<object>>(async (itemSelected) =>
        {
            try
            {
                _locationSelectedValues.Clear();

                foreach (var item in itemSelected)
                {
                    if (item is LocationField selectedItems)
                    {
                        selectedItems.IsSelected = true;
                        _locationSelectedValues.Add(selectedItems);
                    }
                }

                foreach (var item in LocationValues)
                {
                    if (item is LocationField locationField && !_locationSelectedValues.Contains(locationField))
                    {
                        locationField.IsSelected = false;
                    }
                }
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }

        });

        private ObservableCollection<TypeField> _selectedTypes = new ObservableCollection<TypeField>();
        public ObservableCollection<TypeField> SelectedTypes
        {
            get { return _selectedTypes; }
            set
            {
                if (_selectedTypes != value)
                {
                    _selectedTypes = value;
                    OnPropertyChanged(nameof(SelectedTypes));
                }
            }
        }
        private async Task OnCancelAsync()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }
        private async Task OnClearAllAsync()
        {
            try
            {
                await ClearTypeValuesAsync();
                await ClearLocationValues();
                LowerKeepValid = LowerQuantity = 0;
                UpperKeepValid = 10;
                UpperQuantity = 100;
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async Task ClearTypeValuesAsync()
        {
            try
            {
                foreach (var item in TypeValues)
                {
                    item.IsSelected = false;
                }
                TypeSelectedValues.Clear();
                SelectedTypeValues.Clear();
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async Task ClearLocationValues()
        {
            try
            {
                foreach (var item in LocationValues)
                {
                    item.IsSelected = false;
                }
                LocationSelectedValues.Clear();
                SelectedLocationValues.Clear();
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async Task OnApplyAsync()
        {
            try
            {
                FilterInfo filterInfo = new FilterInfo();
                InitializFilterInfo(filterInfo);

                ObservableCollection<DishCard> allDishes = await _foodServices.GetDishCards();
                ObservableCollection<User> allUsers = await _userServices.GetUsers();

                var filterUsers = new ObservableCollection<User>
                    (allUsers.Where(user => filterInfo.LocationValues.Contains(user.Location)));


                ObservableCollection<DishCard> filteredDishes = new ObservableCollection<DishCard>(
                    allDishes.Where(dish =>
                        dish.KeepValid >= filterInfo.LowerKeepValid && dish.KeepValid <= filterInfo.UpperKeepValid &&
                        dish.Quantity >= filterInfo.LowerQuantity && dish.Quantity <= filterInfo.UpperQuantity &&
                        (filterInfo.TypeValues == null || !filterInfo.TypeValues.Any() || filterInfo.TypeValues.Contains(dish.Type)) &&
                        (!filterUsers.Any() || filterUsers.Any(user => user.Id == dish.Restaurant.Id))
                    )
                );

                Preferences.Set("LowerKeepValid", LowerKeepValid);
                Preferences.Set("UpperKeepValid", UpperKeepValid);
                Preferences.Set("LowerQuantity", LowerQuantity);
                Preferences.Set("UpperQuantity", UpperQuantity);


                string typeJson = JsonConvert.SerializeObject(SelectedTypeValues);
                Preferences.Set("SelectedTypeValues", typeJson);

                string locationJson = JsonConvert.SerializeObject(SelectedTypeValues);
                Preferences.Set("SelectedLocationValues", locationJson);

                MessagingCenter.Send<FilterPopUpViewModel, ObservableCollection<DishCard>>(this, "EditDishes", filteredDishes);

                await PopupNavigation.Instance.PopAsync();
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async Task<bool> ValidateUserSession(string id)
        {
            if (id == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Your login session has been expired", "Ok");
                await _navigation.PushAsync(new LoginPage());
                return false;
            }
            return true;
        }

        private void InitializFilterInfo(FilterInfo filterInfo)
        {
            foreach (var item in TypeSelectedValues)
            {
                filterInfo.TypeValues.Add(item.Name);
            }
            foreach (var item in LocationSelectedValues)
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
        public ObservableCollection<TypeField> TypeSelectedValues
        {
            get => _typeSelectedValues;
            set => SetProperty(ref _typeSelectedValues, value);
        }

        private ObservableCollection<LocationField> _locationSelectedValues = new ObservableCollection<LocationField>();
        public ObservableCollection<LocationField> LocationSelectedValues
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