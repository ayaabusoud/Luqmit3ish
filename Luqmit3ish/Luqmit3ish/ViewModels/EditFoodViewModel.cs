using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;

using Xamarin.Essentials;
using Xamarin.Forms;


namespace Luqmit3ish.ViewModels
{
    class EditFoodViewModel : INotifyPropertyChanged
    {
        private int food_id;

        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SubmitCommand { protected set; get; }
        public FoodServices foodServices;

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
            this.food_id = food_id;
            this.Navigation = navigation;

            SubmitCommand = new Command(async () => await OnSubmitClicked());
            foodServices = new FoodServices();
            Photo_clicked = new Command(async () => await PhotoClicked());
            TakePhotoCommand = new Command(async () => await PhotoClicked());
            PlusCommand = new Command(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            PlusCommand1 = new Command(OnPlusClicked1);
            MinusCommand1 = new Command(OnMinusClicked1);
            _typeValues = new ObservableCollection<TypeField>
            {
                 new TypeField { Value = TypeFieldValue.Food, Name = "Food", IconText = "\ue4c6;" },
                new TypeField { Value = TypeFieldValue.Drink, Name = "Drink", IconText = "\uf4e3;" },
                new TypeField { Value = TypeFieldValue.Cake, Name = "Cake", IconText = "\uf1fd;" },
                new TypeField { Value = TypeFieldValue.Snack, Name = "Snack", IconText = "\uf564;" },
                new TypeField { Value = TypeFieldValue.Candies, Name = "Candies", IconText = "\uf786;" },
                new TypeField { Value = TypeFieldValue.Fish, Name = "Fish", IconText = "\uf578;" },
            };
            SelectedType = TypeValues.FirstOrDefault();

            InitializeAsync();


        }

        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }

        private async void InitializeAsync()
        {
            try
            {
                var dishes = await foodServices.GetFoodById(food_id);
                Dishes = new ObservableCollection<Dish>(new List<Dish> { dishes });

                Dish firstDish = Dishes.FirstOrDefault();

                if (firstDish != null)
                {
                    var selectedTypeName = firstDish.type;

                    var selectedType = TypeValues.FirstOrDefault(tf => tf.Name == selectedTypeName);
                    SelectedType = selectedType;
                    _type = firstDish.type;
                    Title = firstDish.name;
                    Description = firstDish.description;
                    Counter = firstDish.keep_listed;
                    Pack_time = firstDish.pick_up_time;
                    Counter1 = firstDish.number;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public ObservableCollection<TypeField> TypeValues1 { get; } = new ObservableCollection<TypeField>();

        private int _counter = 0;
        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
        }

        private int _counter1 = 0;
        public int Counter1
        {
            get => _counter1;
            set => SetProperty(ref _counter1, value);
        }

        private void OnMinusClicked()
        {
            if (Counter == 0)
            {
                return;
            }
            else
            {
                Counter--;
            }

        }

        private void OnPlusClicked1()
        {
            Counter1++;
        }

        private void OnMinusClicked1()
        {
            if (Counter1 == 0)
            {
                return;
            }
            else
            {
                Counter1--;
            }

        }

        private void OnPlusClicked()
        {
            Counter++;
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region type

        public string SelectedTypeName
        {
            get { return SelectedType?.Name; }
        }

        public ICommand MyCollectionSelectedCommand => new Command(() =>
        {
            _type = SelectedTypeName;
        });
        #endregion

        #region color

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                    OnPropertyChanged(nameof(StackLayoutBackgroundColor));
                }
            }
        }


        public Color StackLayoutBackgroundColor => IsSelected ? Color.Blue : Color.White;

        private TypeField selectedType = new TypeField();
        public TypeField SelectedType
        {
            get { return selectedType; }
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));

                    if (selectedType != null)
                    {
                        foreach (var type in TypeValues)
                        {
                            type.IsSelected = type == selectedType;
                        }
                    }
                }
            }
        }


        #endregion
        private ObservableCollection<TypeField> _typeValues;
        public ObservableCollection<TypeField> TypeValues
        {
            get => _typeValues;
            set
            {
                if (_typeValues == value) return;
                _typeValues = value;
                OnPropertyChanged();
            }
        }

        private string proximateNumberValue = "1";
        public string ProximateNumberValue
        {
            get => proximateNumberValue;
            set { SetProperty(ref proximateNumberValue, value); }
        }

        private string keepListedValue = "1";
        public string KeepListedValue
        {
            get => keepListedValue;
            set { SetProperty(ref keepListedValue, value); }
        }

        private ImageSource img;
        public ImageSource Img
        {
            get => img;
            set { SetProperty(ref img, value); }
        }

        private async Task PhotoClicked()
        {

        }

        private async Task OnSubmitClicked()
        {

            try
            {
                if (_type == null || _title == null || _description == null || Counter == 0 || _packTime == null || Counter1 == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please fill in all fields", "ok");
                    return;
                }
                Console.WriteLine("_type " + _type);
                Console.WriteLine("_title " + _title);
                Console.WriteLine("_description " + _description);
                Console.WriteLine("Counter " + Counter);
                Console.WriteLine("_packTime " + _packTime);
                Console.WriteLine("Counter1 " + Counter1);

                string id = Preferences.Get("userId", "0");
                int userId = int.Parse(id);
                Console.WriteLine("food_id " + food_id);
                Console.WriteLine("userId " + userId);
                DishRequest foodRequest = new DishRequest()
                {
                    id = food_id,
                    user_id = userId,
                    photo = "",
                    type = _type,
                    name = _title,
                    description = _description,
                    keep_listed = Counter,
                    pick_up_time = _packTime,
                    number = Counter1
                };

                await UpdateDish(foodRequest);
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

        public async Task<ObservableCollection<Dish>> GetFood(int id)
        {
            try
            {
                ObservableCollection<Dish> request = await foodServices.GetFoodByResId(id);
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
                var request = await foodServices.UpdateDish(foodRequest, food_id);
                if (request)
                {
                    await Navigation.PopAsync();
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

        private string _type;
        public string Type
        {
            get => _type;
            set { SetProperty(ref _type, value); }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set { SetProperty(ref _title, value); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { SetProperty(ref _description, value); }
        }

        private int _keepListed;
        public int Keep_listed
        {
            get => _keepListed;
            set { SetProperty(ref _keepListed, value); }
        }

        private string _packTime;
        public string Pack_time
        {
            get => _packTime;
            set { SetProperty(ref _packTime, value); }
        }

        private int _proximateNumber;
        public int Number
        {
            get => _proximateNumber;
            set { SetProperty(ref _proximateNumber, value); }
        }
    }
}
