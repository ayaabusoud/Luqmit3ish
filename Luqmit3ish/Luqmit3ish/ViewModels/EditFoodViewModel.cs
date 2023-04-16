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

        public EditFoodViewModel(INavigation navigation, int food_id)
        {
            this._foodId = food_id;
            this._navigation = navigation;
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
                var dishes = await _foodServices.GetFoodById(_foodId);
                Dishes = new ObservableCollection<Dish>(new List<Dish> { dishes });

                Dish firstDish = Dishes.FirstOrDefault();

                if (firstDish != null)
                {
                    Type = firstDish.type;
                    Title = firstDish.name;
                    Description = firstDish.description;
                    KeepValid = firstDish.keep_listed;
                    Pack_time = firstDish.pick_up_time;
                    Quantity = firstDish.number;

                    var selectedTypeName = firstDish.type;
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

                await UpdateDish(foodRequest);
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

        public async Task<bool> UpdateDish(DishRequest foodRequest)
        {
            try
            {
                var request = await _foodServices.UpdateDish(foodRequest, _foodId);
                if (request)
                {
                    await _navigation.PopAsync();
                    await App.Current.MainPage.DisplayAlert("Updated successfuly", "the dish update successfuly", "ok");
                    return true;
                }
                await App.Current.MainPage.DisplayAlert("Error", "the dish not added", "ok");
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
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

        private ImageSource _img;
        public ImageSource Img
        {
            get => _img;
            set => SetProperty(ref _img, value);
        }

        private ObservableCollection<Dish> _dishes;
        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
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