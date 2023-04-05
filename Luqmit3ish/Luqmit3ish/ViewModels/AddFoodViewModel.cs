using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
namespace Luqmit3ish.ViewModels
{
    class AddFoodViewModel : BindableObject, INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public FoodServices foodServices;
        public event PropertyChangedEventHandler PropertyChanged;
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

        public AddFoodViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            SubmitCommand = new Command(async () => await OnSubmitClicked());
            foodServices = new FoodServices();
            Photo_clicked = new Command(async () => await PhotoClicked());
            TakePhotoCommand = new Command(async () => await PhotoClicked());
            PlusCommand = new Command(OnPlusClicked);
            MinusCommand = new Command(OnMinusClicked);
            PlusCommand1 = new Command(OnPlusClicked1);
            MinusCommand1 = new Command(OnMinusClicked1);
            typeValues = new ObservableCollection<TypeField>
            {
                new TypeField{ name="Food", iconText="\ue4c6;"},
                new TypeField{ name="Drink", iconText="\uf4e3;"},
                new TypeField{ name="Cake", iconText="\uf1fd;"},
                new TypeField{ name="Snack", iconText="\uf564;"},
                new TypeField{ name="Candies", iconText="\uf786;"},
                new TypeField{ name="Fish", iconText="\uf578"},
            };

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

        private TypeField selectedType;
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
        private ObservableCollection<TypeField> typeValues;
        public ObservableCollection<TypeField> TypeValues
        {
            get => typeValues;
            set
            {
                if (typeValues == value) return;
                typeValues = value;
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
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable
            || CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ": ( No camera available.", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(
            new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
            });
            if (file == null)
                return;
            var img1 = file.AlbumPath;
            Console.WriteLine("imaagee " + img1);
            img = ImageSource.FromStream(() =>
                        {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }


        private async Task OnSubmitClicked()
        {
            try
            {
                if(_type == null || _title == null || _description == null || Counter == 0 || _packTime == null || Counter1 == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please fill in all fields", "ok");
                    return;
                }

                string id = Preferences.Get("userId", "0");
                int userId = int.Parse(id);

                DishRequest foodRequest = new DishRequest()
                {
                   user_id = userId,
                   photo ="",
                   type = _type,
                   name = _title,
                   description = _description,
                   keep_listed = Counter,
                   pick_up_time = _packTime,
                   number = Counter1
                };

                await AddNewDish(foodRequest);

            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task<bool> AddNewDish(DishRequest foodRequest)
        {
            try
            {
                var request = await foodServices.AddNewDish(foodRequest);
                if (request)
                {
                    await App.Current.MainPage.DisplayAlert("Added successfuly", "the dish added successfuly", "ok");
                    await Navigation.PopAsync();
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
