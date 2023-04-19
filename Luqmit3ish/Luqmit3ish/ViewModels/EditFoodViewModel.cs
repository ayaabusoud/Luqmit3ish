using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
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
    class EditFoodViewModel : ViewModelBase
    {
        private int _foodId;
        private Dish _dish;
        private INavigation _navigation;
        private FoodServices _foodServices;

        public ICommand SubmitCommand { protected set; get; }

        public ICommand Photo_clicked { protected set; get; }
        public ICommand Blus { get; private set; }
        public ICommand Minus { protected get; set; }
        public ICommand KeepListedBlus { protected get; set; }
        public ICommand KeepListedMinus { protected get; set; }
        public ICommand TakePhotoCommand { protected set; get; }
        public ICommand PlusCommand { protected set; get; }
        public ICommand MinusCommand { protected set; get; }
        public ICommand PlusCommand1 { protected set; get; }
        public ICommand MinusCommand1 { protected set; get; }

        public EditFoodViewModel(INavigation navigation, Dish dish)
        {
            this._foodId = dish.id;
            this._navigation = navigation;
            _dish = dish;
            _foodServices = new FoodServices();

            SubmitCommand = new Command(async () => await OnSubmitClicked());
            Photo_clicked = new Command(async () => await PhotoClicked());
            TakePhotoCommand = new Command(async () => await PhotoClicked());

            PlusCommand = new Command(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            PlusCommand1 = new Command(OnPlusClicked1);
            MinusCommand1 = new Command(OnMinusClicked1);

            InitializeTypeValues();
            SelectedType = TypeValues.FirstOrDefault();
            InitializeAsync();
        }

        private void InitializeTypeValues()
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

        private async void InitializeAsync()
        {
            try
            {
                if (_dish != null)
                {
                    Type = _dish.type;
                    Title = _dish.name;
                    Description = _dish.description;
                    KeepValid = _dish.keep_listed;
                    Pack_time = _dish.pick_up_time;
                    Quantity = _dish.number;

                    var selectedTypeName = _dish.type;
                    var selectedType = TypeValues.FirstOrDefault(tf => tf.Name == selectedTypeName);
                    SelectedType = selectedType;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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

        private void OnMinusClicked()
        {
            if (KeepValid > 0)
            {
                KeepValid--;
            }
        }

        private void OnPlusClicked1()
        {
            Quantity++;
        }

        private void OnMinusClicked1()
        {
            if (Quantity > 0)
            {
                Quantity--;
            }
        }

        private void OnPlusClicked()
        {
            KeepValid++;
        }

        public string SelectedTypeName
        {
            get { return SelectedType?.Name; }
        }

        public ICommand MyCollectionSelectedCommand => new Command(() =>
        {
            _type = SelectedTypeName;
        });

        private async Task PhotoClicked()
        {
            try
            {
                bool userSelect = await App.Current.MainPage.DisplayAlert("Upload Image", "", "Take photo", "select from Gallary");

                if (userSelect)
                {
                    TakePhoto();
                }
                else
                {
                    SelectFromGallary();
                }
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await App.Current.MainPage.DisplayAlert("Error", "There was a problem with your internet connection.", "OK");
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await App.Current.MainPage.DisplayAlert("Error", "Unable to connect to the server. Please check your internet connection and try again.", "OK");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async void TakePhoto()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take Photo"
                });

                if (result != null)
                {
                    _photoPath = result.FullPath;
                }
                else
                {
                    _photoPath = string.Empty;
                }
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

        private async void SelectFromGallary()
        {
            try
            {
                await Permissions.RequestAsync<Permissions.Photos>();

                var result = await MediaPicker.PickPhotoAsync();

                if (result != null)
                {
                    _photoPath = result.FullPath;
                }
                else
                {
                    Console.WriteLine("User cancelled photo picker.");
                }
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

        private async Task OnSubmitClicked()
        {
            try
            {
                string id = Preferences.Get("userId", "0");

                if (id is null)
                {
                    return;
                }
                int userId = int.Parse(id);

                if (_type == null || _title == null || _description == null || KeepValid == 0 || _packTime == null || Quantity == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please fill in all fields", "ok");
                    return;
                }

                DishRequest foodRequest = new DishRequest()
                {
                    id = _foodId,
                    user_id = userId,
                    photo = "",
                    type = _type,
                    name = _title,
                    description = _description,
                    keep_listed = KeepValid,
                    pick_up_time = _packTime,
                    number = Quantity
                };

                UpdateDish(foodRequest);
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await App.Current.MainPage.DisplayAlert("Error", "There was a problem with your internet connection.", "OK");
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await App.Current.MainPage.DisplayAlert("Error", "Unable to connect to the server. Please check your internet connection and try again.", "OK");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task<ObservableCollection<Dish>> GetFood(int id)
        {
            try
            {
                ObservableCollection<Dish> request = await _foodServices.GetFoodByResId(id);
                if (request != null)
                {
                    return request;
                }
                await App.Current.MainPage.DisplayAlert("Error", "the dish not updated", "ok");
                return null;
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "ok");
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async void UpdateDish(DishRequest foodRequest)
        {
            try
            {
                var request = await _foodServices.UpdateDish(foodRequest, _foodId);
                if (request)
                {
                    await AddNewPhoto(_photoPath, _foodId);
                    return;
                }
                await App.Current.MainPage.DisplayAlert("Error", "the dish not added", "ok");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async Task AddNewPhoto(string photoPath, int foodId)
        {
            var response = await _foodServices.UploadPhoto(photoPath, foodId);

            if (response)
            {
                await _navigation.PopAsync();
                await App.Current.MainPage.DisplayAlert("Updated successfuly", "the dish update successfuly", "ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "The dish was not added", "OK");
            }
        }

        private TypeField _selectedType = new TypeField();
        public TypeField SelectedType
        {
            get => _selectedType;
            set
            {
                SetProperty(ref _selectedType, value);
                if (_selectedType != null)
                {
                    foreach (var type in TypeValues)
                    {
                        type.IsSelected = type == _selectedType;
                    }
                }
            }
        }

        private ObservableCollection<TypeField> _typeValues;
        public ObservableCollection<TypeField> TypeValues
        {
            get => _typeValues;
            set => SetProperty(ref _typeValues, value);
        }

        private string _proximateNumberValue = "1";
        public string ProximateNumberValue
        {
            get => _proximateNumberValue;
            set => SetProperty(ref _proximateNumberValue, value);
        }

        private string _keepListedValue = "1";
        public string KeepListedValue
        {
            get => _keepListedValue;
            set => SetProperty(ref _keepListedValue, value);
        }

        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set => SetProperty(ref _photoPath, value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

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

        private int _keepListed;
        public int Keep_listed
        {
            get => _keepListed;
            set => SetProperty(ref _keepListed, value);
        }

        private string _packTime;
        public string Pack_time
        {
            get => _packTime;
            set => SetProperty(ref _packTime, value);
        }

        private int _proximateNumber;
        public int Number
        {
            get => _proximateNumber;
            set => SetProperty(ref _proximateNumber, value);
        }
    }
}