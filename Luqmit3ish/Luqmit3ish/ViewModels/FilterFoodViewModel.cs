using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class FilterFoodViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        public ICommand Apply { get; set; }
        public ICommand ClearAll { get; set; }
        private FoodServices _foodServices;
        private UserServices _userServices;

        public FilterFoodViewModel(INavigation navigation)
        {
            SelectedTypeValues = new ObservableCollection<object>();
            this._navigation = navigation;

            InitializTypeValues();
            InitializLocationValues();

            Apply = new Command(async () => await OnApplyAsync());
            ClearAll = new Command(OnClearAll);
            _foodServices = new FoodServices();
            _userServices = new UserServices();
            _upperQuantity = 100;
            _upperKeepValid = 10;
            _lowerQuantity = _lowerKeepValid = 0;
            
        }

        private void InitializTypeValues()
        {
            _typeValues = new ObservableCollection<TypeField>
            {
                new TypeField { Name = "Food", IconText = "\ue4c6;" },
                new TypeField { Name = "Drink", IconText = "\uf4e3;" },
                new TypeField { Name = "Cake", IconText = "\uf7ef;" },
                new TypeField { Name = "Snack", IconText = "\uf787;" },
                new TypeField { Name = "Candies", IconText = "\uf786;" },
                new TypeField { Name = "Fish", IconText = "\uf578;" }
            };
        }

        private void InitializLocationValues()
        {
            _locationValues = new ObservableCollection<LocationField>
            {
                 new LocationField{ Name="Nablus" },
                 new LocationField{ Name="Ramallah" },
                 new LocationField{ Name="Jenin" },
                 new LocationField{ Name="Jericho" },
                 new LocationField{ Name="Jerusalem" },
                 new LocationField{ Name="Tulkarm" },
            };
        }

        public ICommand TypeMultiSelectionCommand => new Command<IList<object>>((itemSelected) =>
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
        });

        public ICommand LocationMultiSelectionCommand => new Command<IList<object>>((itemSelected) =>
        {
            _locationSelectedValues.Clear();

            foreach (var item in itemSelected)
            {
                if (item is LocationField selectedItems)
                {
                    selectedItems.IsSelected = true;

                    if (selectedItems.IsSelected)
                    {
                        _locationSelectedValues.Add(selectedItems);
                    }
                }
            }

            foreach (var item in LocationValues)
            {
                if (item is LocationField locationField && !_locationSelectedValues.Contains(locationField))
                {
                    locationField.IsSelected = false;
                }
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

        private void OnClearAll()
        {

            ClearTypeValues();
            ClearLocationValues();
            LowerKeepValid = LowerQuantity = 0;
            UpperKeepValid = 10;
            UpperQuantity = 100;
        }

        private void ClearTypeValues()
        {
            foreach (var item in TypeValues)
            {
                item.IsSelected = false;
            }
            TypeSelectedValues.Clear();
            TypeMultiSelectionCommand.Execute(new List<object>());

            if (ClearAll == null)
            {
                ClearAll = new Command<IList>(items =>
                {
                    // Other code...
                    SelectedTypeValues.Clear(); // Deselect all items
                });
            }

        }

        private void ClearLocationValues()
        {
            foreach (var item in LocationValues)
            {
                item.IsSelected = false;
            }
            LocationSelectedValues.Clear();
        }

        private async Task OnApplyAsync()
        {
            try
            {
                var id = Preferences.Get("userId", null);
                bool isValidUserSession = await ValidateUserSession(id);
                if (!isValidUserSession)
                {
                    return;
                }

                int userId = int.Parse(id);

                FilterInfo filterInfo = new FilterInfo();
                InitializFilterInfo(filterInfo);

                ObservableCollection<DishCard> allDishes = await _foodServices.GetDishCards();
                ObservableCollection<User> allUsers = await _userServices.GetUsers();

                var filterUsers = new ObservableCollection<User>
                    (allUsers.Where(user => filterInfo.LocationValues.Contains(user.Location)));


                ObservableCollection<DishCard> filterDishes = new ObservableCollection<DishCard>
                    (allDishes.Where(dish =>
                        (dish.keepValid >= filterInfo.LowerKeepValid && dish.keepValid <= filterInfo.UpperKeepValid) &&
                        (dish.quantity >= filterInfo.LowerQuantity && dish.quantity <= filterInfo.UpperQuantity) &&
                        (filterInfo.TypeValues == null || !filterInfo.TypeValues.Any() || filterInfo.TypeValues.Contains(dish.type)) &&
                        ((filterUsers.Count == 0) || (filterUsers.Any(user => user.id == dish.restaurantId)))
                        )
                    ) ;

                MessagingCenter.Send<FilterFoodViewModel, ObservableCollection<DishCard>>(this, "EditDishes", filterDishes);
                await _navigation.PopAsync();

            }
            catch (ConnectionException)
            {
                await Application.Current.MainPage.DisplayAlert("Bad Request", "Please check your connection", "Ok");
            }
            catch (HttpRequestException)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went bad on this reservation, you can try again", "Ok");
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "ok");
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

        private LocationField _selectedLocation;
        public LocationField SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                SetProperty(ref _selectedLocation, value);

                if (_selectedLocation != null)
                {
                    foreach (var location in LocationValues)
                    {
                        location.IsSelected = location == _selectedLocation;
                    }
                }
            }
        }

        public TypeField SelectedType
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    value.IsSelected = true;
                }
            }
        }
    }
}
