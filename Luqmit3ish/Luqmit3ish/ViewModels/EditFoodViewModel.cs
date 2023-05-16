using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Utilities;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    class EditFoodViewModel : ViewModelBase
    {
        private INavigation _navigation;
        private FoodServices _foodServices;

        public ICommand SubmitCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }
        public ICommand TakePhotoCommand { protected set; get; }
        public ICommand KeepValidPlusCommand { protected set; get; }
        public ICommand KeepValidMinusCommand { protected set; get; }
        public ICommand QuantityPlusCommand { protected set; get; }
        public ICommand QuantityMinusCommand { protected set; get; }

        public EditFoodViewModel(INavigation navigation, Dish dish)
        {
            _navigation = navigation;
            _dish = dish;
            _foodServices = new FoodServices();

            SubmitCommand = new Command(async () => await OnSubmitClicked());
            DeleteCommand = new Command(async () => await OnDeleteClicked());
            TakePhotoCommand = new Command(async () => await PhotoClicked());
            KeepValidPlusCommand = new Command(OnKeepValidPlusClicked);
            KeepValidMinusCommand = new Command(OnKeepValidMinusClicked);
            QuantityPlusCommand = new Command(OnQuantityPlusClicked);
            QuantityMinusCommand = new Command(OnQuantityMinusClicked);

            _typeValues = Constants.TypeValues;
            SelectedType = TypeValues.FirstOrDefault();
            SelectedType = TypeValues.FirstOrDefault(tf => tf.Name == _dish.Type);
            _keepValid = _dish.KeepValid;
            _quantity = _dish.Quantity;

            MessagingCenter.Subscribe<UploadImagePopUpViewModel, string>(this, "PhotoPath", (sender, photoPath) =>
            {
                _dish.Photo = photoPath;
                OnPropertyChanged(nameof(DishInfo));
            });
        }

        private Dish _dish;
        public Dish DishInfo
        {
            get => _dish;
            set => SetProperty(ref _dish, value);
        }

        private int _keepValid = 0;
        public int KeepValid
        {
            get => _keepValid;
            set => SetProperty(ref _keepValid, value);
        }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        private ObservableCollection<TypeField> _typeValues;
        public ObservableCollection<TypeField> TypeValues
        {
            get => _typeValues;
            set => SetProperty(ref _typeValues, value);
        }

        private TypeField _selectedType = new TypeField();
        public TypeField SelectedType
        {
            get => _selectedType;
            set
            {
                SetProperty(ref _selectedType, value);
                UpdateTypeSelection();
            }
        }

        private void UpdateTypeSelection()
        {
            if (_selectedType != null)
            {
                foreach (var type in TypeValues)
                {
                    type.IsSelected = type == _selectedType;
                }
            }
        }

        public string SelectedTypeName
        {
            get => SelectedType?.Name;
        }

        public ICommand MyCollectionSelectedCommand => new Command(() =>
        {
            _dish.Type = SelectedTypeName;
        });

        private void OnKeepValidMinusClicked()
        {
            if (KeepValid > 0)
            {
                KeepValid--;
            }
        }

        private void OnQuantityPlusClicked()
        {
            Quantity++;
        }

        private void OnQuantityMinusClicked()
        {
            if (Quantity > 1)
            {
                Quantity--;
            }
        }

        private void OnKeepValidPlusClicked()
        {
            KeepValid++;
        }

        private async Task PhotoClicked()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new UploadImagePopUp());
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

            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(NotAuthorizedMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task OnDeleteClicked()
        {
            try
            {
                var deleteConfirm = await Application.Current.MainPage.DisplayAlert("Delete dish", "Are you sure that you want to delete this dish?", "Yes", "No");
                if (deleteConfirm)
                {
                    await _foodServices.DeleteFood(_dish.Id);
                    await _navigation.PopAsync();
                    await PopNavigationAsync("The Dish have been deleted successfully.");
                    return;
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
                await PopNavigationAsync(HttpRequestMessage);
            }
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(NotAuthorizedMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task OnSubmitClicked()
        {
            try
            {
                if (!IsDishDataValid())
                {
                    await PopNavigationAsync("Please fill in all fields.");
                    return;
                }
                UpdateDish(DishInfo);
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

        private bool IsDishDataValid()
        {
            return (_dish.Type != null && _dish.Name != null && _dish.Description != null && _dish.KeepValid > 0 && _dish.Quantity > 0);
        }

        public async void UpdateDish(Dish foodRequest)
        {
            try
            {
                _dish.Quantity = Quantity;
                _dish.KeepValid = KeepValid;
                var request = await _foodServices.UpdateDish(foodRequest);
                if (request)
                {
                    await _navigation.PopAsync();
                    await PopNavigationAsync("The Dish have been updated successfully.");
                    return;
                }
                await PopNavigationAsync(ExceptionMessage);
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
    }
}